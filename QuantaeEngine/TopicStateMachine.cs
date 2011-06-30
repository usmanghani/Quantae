using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public static SentenceHandle GetNextSentence(UserProfile profile, Tristate success)
        {
            if (profile.CurrentState.TopicStateMachineState.CurrentSection == TopicSectionType.SentenceAndQuestion)
            {
                UpdateUserProfileWithCurrentSentenceResponse(profile, success);
            }

            return null;
        }

        private static void UpdateUserProfileWithCurrentSentenceResponse(UserProfile profile, Tristate success)
        {
            var topicStateMachineState = profile.CurrentState.TopicStateMachineState;
            var sampleSectionState = topicStateMachineState.SampleSectionState;
            var currentSentence = Repositories.Repositories.Sentences.GetItemByHandle(sampleSectionState.CurrentSentence);

            if (profile.CurrentState.TopicStateMachineState.SampleSectionState.IsQuestion)
            {
                switch(profile.CurrentState.TopicStateMachineState.SampleSectionState.CurrentQuestionDimension)
                {
                    case QuestionDimension.Vocab:
                        VocabOperations.UpdateVocabulary(profile, currentSentence.VocabEntries, VocabRankTypes.SeenInSampleOrQuestion, success);
                        break;
                    case QuestionDimension.Understanding:

                        break;
                    case QuestionDimension.Grammar:
                        break;
                    case QuestionDimension.NounConjugation:
                    case QuestionDimension.VerbConjugation:
                        break;
                    case QuestionDimension.Revision:
                        break;
                }
            }

            // this means we are just starting this section.
            if (profile.CurrentState.TopicStateMachineState.SampleSectionIterationCount == 0)
            {

            }
        }
    }
}
