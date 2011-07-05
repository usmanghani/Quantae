using Quantae.DataModel;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Quantae.Engine
{
    public interface ISentenceSelectionEngine
    {
        Sentence GetNextSentence(UserProfile profile);
    }

    public interface IUserProfileManager
    {
        void SubmitSentenceResponse(AnswerDimension answerDimension, AnswerScore score);
    }

    public class SentenceSelectionEngine
    {
        // Take it out.
        public static TopicSectionType GetCurrentSection(UserProfile userProfile)
        {
            if (userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection == TopicSectionType.Intro &&
                userProfile.CurrentState.CourseLocationInfo.CurrentTopic.IsPseudoTopic)
            {
                userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection = TopicSectionType.Exercise;
            }

            return userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection;
        }

        // This is out also.
        public static string GetNextIntroSlideContent(UserProfile userProfile)
        {
            Topic currentTopic = Repositories.Repositories.Topics.GetItemByHandle(userProfile.CurrentState.CourseLocationInfo.CurrentTopic.Topic);

            if (userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection == TopicSectionType.Intro &&
               !userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IsIntroComplete &&
                userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IntroSlideIndex < currentTopic.IntroSection.Pages.Count)
            {
                int idx = ++userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IntroSlideIndex;

                // BUG: Separate this out.
                VocabOperations.UpdateVocabulary(userProfile, currentTopic.IntroSection.Pages[idx].VocabEntries, VocabRankTypes.CorrectOrSeenInIntro);
                return currentTopic.IntroSection.Pages[idx].Content;
            }

            userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IsIntroComplete = true;
            userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection = TopicSectionType.Exercise;

            return string.Empty;
        }

        public static Sentence GetNextSentence(UserProfile profile, AnswerDimension answerDimension, AnswerScore score)
        {
            // ALGO: 
            // PRE: Session exists
            // PRE: We are in Exercise or Review Section.
            // What we need: 
            // 1. All histories
            // 2. All user current state.
            // 3. Access to sentence repository.
            // What do we do:
            // 1. Assert correct section.
            // 2. Handle Exercise section. OR
            // 3. Handle Review Section.

            var currentSection = profile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection;
            if(currentSection != TopicSectionType.Review && currentSection != TopicSectionType.Exercise)
            {
                // BUG: Clean this up.
                throw new Exception("Invalid section");
            }

            Sentence targetSentence = null;

            if(currentSection == TopicSectionType.Exercise)
            {
                targetSentence = HandleExerciseSection(profile);
            }
            else
            {
                targetSentence = HandleReviewSection(profile);
            }

            return targetSentence;

            //var topicLocationInfo = profile.CurrentState.CourseLocationInfo.TopicLocationInfo;
            //var exerciseSectionState = topicLocationInfo.ExerciseSectionState;
            //var currentSentence = Repositories.Repositories.Sentences.GetItemByHandle(exerciseSectionState.CurrentSentence);

            //Sentence targetSentence = null;

            //// this means we are just starting this section.
            //// TODO: Figure out if there is something special to do at the start of the section.
            ////if (profile.CurrentState.TopicStateMachineState.SampleSectionIterationCount == 0)
            ////{

            ////}

            //// TODO: Convert these to Asserts.
            //if (profile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection == TopicSectionType.Exercise ||
            //    profile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection == TopicSectionType.Review)
            //{
            //    // BUG: BUG: BUG: Figure out transactionality of updates throughout the system.
            //    // TODO: Figure out double updates or repeated updates to the same entity.
            //    // TODO: Move this to its rightful place. 
            //    UpdateUserProfileWithCurrentSentenceResponse(profile, answerDimension, score);

            //    // TODO: More stuff here.
            //    // Figure out the next sentence and its mode.

            //    // This wasnt the question, so lets start asking questions now.
            //    if (!exerciseSectionState.IsQuestion)
            //    {
            //        exerciseSectionState.IsQuestion = true;

            //        if (topicLocationInfo.CurrentSection == TopicSectionType.Review)
            //        {
            //            exerciseSectionState.CurrentQuestionDimension = QuestionDimension.Understanding;
            //        }
            //        else
            //        {
            //            exerciseSectionState.CurrentQuestionDimension = (QuestionDimension)1;

            //            // for the first question we are going to re-use this sentence.
            //            targetSentence = SentenceOperations.GetSentenceFromHandle(exerciseSectionState.CurrentSentence);
            //        }
            //    }
            //    else
            //    {
            //        topicLocationInfo.QuestionCount++;
            //        if (topicLocationInfo.CurrentSection != TopicSectionType.Review)
            //        {
            //            if ((int)exerciseSectionState.CurrentQuestionDimension < Enum.GetValues(typeof(QuestionDimension)).Length - 1)
            //            {
            //                exerciseSectionState.CurrentQuestionDimension = (QuestionDimension)((int)exerciseSectionState.CurrentQuestionDimension + 1);
            //            }
            //            else
            //            {
            //                topicLocationInfo.ExerciseSectionIterationCount++;
            //                topicLocationInfo.ExerciseSectionState = new ExerciseSectionState();
            //            }
            //        }

            //        if (TopicPolicies.IsSampleSectionComplete(profile))
            //        {
            //            if (!profile.CurrentState.CourseLocationInfo.CurrentTopic.IsPseudoTopic)
            //            {
            //                topicLocationInfo.CurrentSection = TopicSectionType.Review;
            //            }
            //        }

            //        // TODO: Do sentence selection logic here.

            //        if (targetSentence == null)
            //        {
            //            targetSentence = SentenceOperations.FindSentence(profile);

            //            // TODO: What happens when we run out of sentences.
            //        }
            //    }
            //}

            //profile.CurrentState.CourseLocationInfo.TopicLocationInfo.ExerciseSectionState.CurrentSentence = new SentenceHandle(targetSentence);

            //return targetSentence;
        }

        private static Sentence HandleExerciseSection(UserProfile profile)
        {
            // PRE: By the time we come here. we have already been moved to either
            // starting next pack or being in a valid question dimension by the state evaluator
            // if starting exercise pack
            //       select sample sentence.
            //       return it.
            // else
            //       select sentence for this question dimension.
            //       return it.

            return null;
        }

        private static Sentence HandleReviewSection(UserProfile profile)
        {
            // PRE: WE have already been moved to the review section by the state evaluator.
            // select sentence for review.
            // return it.

            return null;
        }

        // Make a UserProfileManager and move this there along with other update functionality related to the user.
        private static void UpdateUserProfileWithCurrentSentenceResponse(UserProfile profile, AnswerDimension answerDimension, AnswerScore score)
        {
            var topicStateMachineState = profile.CurrentState.CourseLocationInfo.TopicLocationInfo;
            var sampleSectionState = topicStateMachineState.ExerciseSectionState;
            var currentSentence = Repositories.Repositories.Sentences.GetItemByHandle(sampleSectionState.CurrentSentence);

            var currentSentenceHistoryItem = profile.History.SentenceHistory.Find(shi => shi.Sentence.Equals(sampleSectionState.CurrentSentence));

            if (currentSentenceHistoryItem == null)
            {
                currentSentenceHistoryItem = new SentenceHistoryItem() { Sentence = sampleSectionState.CurrentSentence };
                profile.History.SentenceHistory.Insert(0, currentSentenceHistoryItem);
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

            if (profile.CurrentState.CourseLocationInfo.TopicLocationInfo.ExerciseSectionState.IsQuestion)
            {
                TopicOperations.UpdateAnswerDimensionCounts(profile, answerDimension, score);

                if (sampleSectionState.CurrentQuestionDimension == QuestionDimension.Grammar)
                {
                    LearningTypeOperations.UpdateLearningTypeScore(profile, score);
                }
            }
        }
    }
}
