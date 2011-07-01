using Quantae.DataModel;

namespace Quantae.Engine
{
    public class TopicStateMachine
    {
        public static TopicSectionType GetCurrentSection(UserProfile userProfile)
        {
            if (userProfile.CurrentState.TopicStateMachineState.CurrentSection == TopicSectionType.Intro &&
                userProfile.CurrentState.CourseStateMachineState.CurrentTopic.IsPseudoTopic)
            {
                userProfile.CurrentState.TopicStateMachineState.CurrentSection = TopicSectionType.SentenceAndQuestion;
            }

            return userProfile.CurrentState.TopicStateMachineState.CurrentSection;
        }

        public static string GetNextIntroSlideContent(UserProfile userProfile)
        {
            Topic currentTopic = Repositories.Repositories.Topics.GetItemByHandle(userProfile.CurrentState.CourseStateMachineState.CurrentTopic.Topic);

            if (userProfile.CurrentState.TopicStateMachineState.CurrentSection == TopicSectionType.Intro &&
               !userProfile.CurrentState.TopicStateMachineState.IsIntroComplete &&
                userProfile.CurrentState.TopicStateMachineState.IntroSlideIndex < currentTopic.IntroSection.Slides.Count)
            {
                int idx = ++userProfile.CurrentState.TopicStateMachineState.IntroSlideIndex;
                VocabOperations.UpdateVocabulary(userProfile, currentTopic.IntroSection.Slides[idx].VocabEntries, VocabRankTypes.CorrectOrSeenInIntro);
                return currentTopic.IntroSection.Slides[idx].Content;
            }

            userProfile.CurrentState.TopicStateMachineState.IsIntroComplete = true;
            userProfile.CurrentState.TopicStateMachineState.CurrentSection = TopicSectionType.SentenceAndQuestion;

            return string.Empty;
        }

        public static SentenceHandle GetNextSentence(UserProfile profile, AnswerDimension answerDimension, AnswerScore score)
        {
            // this means we are just starting this section.
            // TODO: Figure out if there is something special to do at the start of the section.
            if (profile.CurrentState.TopicStateMachineState.SampleSectionIterationCount == 0)
            {

            }

            if (profile.CurrentState.TopicStateMachineState.CurrentSection == TopicSectionType.SentenceAndQuestion ||
                profile.CurrentState.TopicStateMachineState.CurrentSection == TopicSectionType.Revision)
            {
                UpdateUserProfileWithCurrentSentenceResponse(profile, answerDimension, score);

                // TODO: More stuff here.


            }

            return null;
        }

        private static void UpdateUserProfileWithCurrentSentenceResponse(UserProfile profile, AnswerDimension answerDimension, AnswerScore score)
        {
            var topicStateMachineState = profile.CurrentState.TopicStateMachineState;
            var sampleSectionState = topicStateMachineState.SampleSectionState;
            var currentSentence = Repositories.Repositories.Sentences.GetItemByHandle(sampleSectionState.CurrentSentence);

            var currentSentenceHistoryItem = profile.SentenceHistory.Find(shi => shi.Sentence.Equals(sampleSectionState.CurrentSentence));

            if (currentSentenceHistoryItem == null)
            {
                currentSentenceHistoryItem = new SentenceHistoryItem() { Sentence = sampleSectionState.CurrentSentence };
                profile.SentenceHistory.Insert(0, currentSentenceHistoryItem);
            }

            HistoryItemOperations.UpdateHistoryItemWithSuccessFailureAndTimestamp(currentSentenceHistoryItem, score);
            VocabOperations.UpdateVocabulary(profile, currentSentence.VocabEntries, VocabRankTypes.SeenInSampleOrQuestion, score);
            NounConjugationOperations.UpdateNounConjugationHistoryFromSentence(profile, currentSentence, score);
            VerbConjugationOperations.UpdateVerbConjugationHistoryFromSentence(profile, currentSentence, score);

            if (profile.CurrentState.TopicStateMachineState.SampleSectionState.IsQuestion)
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
