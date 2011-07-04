using Quantae.DataModel;
using System;
using System.Linq;

namespace Quantae.Engine
{
    public class TopicStateMachine
    {
        public static TopicSectionType GetCurrentSection(UserProfile userProfile)
        {
            if (userProfile.CurrentState.TopicLocationInfo.CurrentSection == TopicSectionType.Intro &&
                userProfile.CurrentState.CourseLocationInfo.CurrentTopic.IsPseudoTopic)
            {
                userProfile.CurrentState.TopicLocationInfo.CurrentSection = TopicSectionType.Exercise;
            }

            return userProfile.CurrentState.TopicLocationInfo.CurrentSection;
        }

        public static string GetNextIntroSlideContent(UserProfile userProfile)
        {
            Topic currentTopic = Repositories.Repositories.Topics.GetItemByHandle(userProfile.CurrentState.CourseLocationInfo.CurrentTopic.Topic);

            if (userProfile.CurrentState.TopicLocationInfo.CurrentSection == TopicSectionType.Intro &&
               !userProfile.CurrentState.TopicLocationInfo.IsIntroComplete &&
                userProfile.CurrentState.TopicLocationInfo.IntroSlideIndex < currentTopic.IntroSection.Slides.Count)
            {
                int idx = ++userProfile.CurrentState.TopicLocationInfo.IntroSlideIndex;

                // BUG: Separate this out.
                VocabOperations.UpdateVocabulary(userProfile, currentTopic.IntroSection.Slides[idx].VocabEntries, VocabRankTypes.CorrectOrSeenInIntro);
                return currentTopic.IntroSection.Slides[idx].Content;
            }

            userProfile.CurrentState.TopicLocationInfo.IsIntroComplete = true;
            userProfile.CurrentState.TopicLocationInfo.CurrentSection = TopicSectionType.Exercise;

            return string.Empty;
        }

        public static Sentence GetNextSentence(UserProfile profile, AnswerDimension answerDimension, AnswerScore score)
        {
            var topicStateMachineState = profile.CurrentState.TopicLocationInfo;
            var sampleSectionState = topicStateMachineState.SampleSectionState;
            var currentSentence = Repositories.Repositories.Sentences.GetItemByHandle(sampleSectionState.CurrentSentence);

            Sentence targetSentence = null;

            // this means we are just starting this section.
            // TODO: Figure out if there is something special to do at the start of the section.
            //if (profile.CurrentState.TopicStateMachineState.SampleSectionIterationCount == 0)
            //{

            //}

            if (profile.CurrentState.TopicLocationInfo.CurrentSection == TopicSectionType.Exercise ||
                profile.CurrentState.TopicLocationInfo.CurrentSection == TopicSectionType.Review)
            {
                // BUG: BUG: BUG: Figure out transactionality of updates throughout the system.
                // TODO: Figure out double updates or repeated updates to the same entity.
                UpdateUserProfileWithCurrentSentenceResponse(profile, answerDimension, score);

                // TODO: More stuff here.
                // Figure out the next sentence and its mode.

                // This wasnt the question, so lets start asking questions now.
                if (!sampleSectionState.IsQuestion)
                {
                    sampleSectionState.IsQuestion = true;

                    if (topicStateMachineState.CurrentSection == TopicSectionType.Review)
                    {
                        sampleSectionState.CurrentQuestionDimension = QuestionDimension.Understanding;
                    }
                    else
                    {
                        sampleSectionState.CurrentQuestionDimension = (QuestionDimension)1;

                        // for the first question we are going to re-use this sentence.
                        targetSentence = SentenceOperations.GetSentenceFromHandle(sampleSectionState.CurrentSentence);
                    }
                }
                else
                {
                    topicStateMachineState.QuestionCount++;
                    if (topicStateMachineState.CurrentSection != TopicSectionType.Review)
                    {
                        if ((int)sampleSectionState.CurrentQuestionDimension < Enum.GetValues(typeof(QuestionDimension)).Length - 1)
                        {
                            sampleSectionState.CurrentQuestionDimension = (QuestionDimension)((int)sampleSectionState.CurrentQuestionDimension + 1);
                        }
                        else
                        {
                            topicStateMachineState.SampleSectionIterationCount++;
                            topicStateMachineState.SampleSectionState = new SampleSectionState();
                        }
                    }

                    if (TopicPolicies.IsSampleSectionComplete(profile))
                    {
                        if (!profile.CurrentState.CourseLocationInfo.CurrentTopic.IsPseudoTopic)
                        {
                            topicStateMachineState.CurrentSection = TopicSectionType.Review;
                        }
                    }

                    // TODO: Do sentence selection logic here.

                    if (targetSentence == null)
                    {
                        targetSentence = SentenceOperations.FindSentence(profile);

                        // TODO: What happens when we run out of sentences.
                    }
                }
            }

            profile.CurrentState.TopicLocationInfo.SampleSectionState.CurrentSentence = new SentenceHandle(targetSentence);

            return targetSentence;
        }

        private static void UpdateUserProfileWithCurrentSentenceResponse(UserProfile profile, AnswerDimension answerDimension, AnswerScore score)
        {
            var topicStateMachineState = profile.CurrentState.TopicLocationInfo;
            var sampleSectionState = topicStateMachineState.SampleSectionState;
            var currentSentence = Repositories.Repositories.Sentences.GetItemByHandle(sampleSectionState.CurrentSentence);

            var currentSentenceHistoryItem = profile.SentenceHistory.Find(shi => shi.Sentence.Equals(sampleSectionState.CurrentSentence));

            if (currentSentenceHistoryItem == null)
            {
                currentSentenceHistoryItem = new SentenceHistoryItem() { Sentence = sampleSectionState.CurrentSentence };
                profile.SentenceHistory.Insert(0, currentSentenceHistoryItem);
            }

            // TODO: Potential pitfall to investigate. 
            // First question always re-uses the existing sentence. This will lead to double updates.
            HistoryItemOperations.UpdateHistoryItemWithSuccessFailureAndTimestamp(currentSentenceHistoryItem, score);
            VocabOperations.UpdateVocabulary(profile, currentSentence.VocabEntries, VocabRankTypes.SeenInSampleOrQuestion, score);
            NounConjugationOperations.UpdateNounConjugationHistoryFromSentence(profile, currentSentence, score);
            VerbConjugationOperations.UpdateVerbConjugationHistoryFromSentence(profile, currentSentence, score);

            if (NounConjugationPolicies.CanMoveToNextNounConjugation(profile))
            {
                profile.CurrentState.CurrentNounConjugationRank++;
            }

            foreach (var tense in profile.CurrentState.CurrentVerbConjugationRanksByTense.Keys)
            {
                if (VerbConjugationPolicies.CanMoveToNextVerbConjugation(profile, tense))
                {
                    profile.CurrentState.CurrentVerbConjugationRanksByTense[tense]++;
                }
            }

            if (profile.CurrentState.TopicLocationInfo.SampleSectionState.IsQuestion)
            {
                TopicOperations.UpdateAnswerDimensionCounts(profile, answerDimension, score);

                if( sampleSectionState.CurrentQuestionDimension ==  QuestionDimension.Grammar)
                {
                    LearningTypeOperations.UpdateLearningTypeScore(profile, score);
                }
            }
        }
    }
}
