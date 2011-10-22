using Quantae.DataModel;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Quantae.Engine
{
    public interface ISentenceSelectionEngine
    {
        GetNextSentenceResult GetNextSentence(UserProfile profile);
    }

    public class SentenceSelectionEngine : ISentenceSelectionEngine
    {
        public GetNextSentenceResult GetNextSentence(UserProfile profile)
        {
            // ALGO: GetNextSentence
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

            GetNextSentenceResult result = null;
            if(currentSection == TopicSectionType.Exercise)
            {
                result = HandleExerciseSection(profile);
            }
            else
            {
                result = HandleReviewSection(profile);
            }

            return result;

        }

        private GetNextSentenceResult HandleExerciseSection(UserProfile profile)
        {
            // PRE: By the time we come here. we have already been moved to either
            // starting next pack or being in a valid question dimension by the state evaluator
            // if starting exercise pack
            //       select sample sentence.
            //       return it.
            // else
            //       select sentence for this question dimension.
            //       return it.

            Sentence sentence = null;
            bool isQuestion = false;
            QuestionDimension dimension = QuestionDimension.Unknown;
            bool found = false;

            sentence = FindSentence(profile);

            if(sentence != null)
            {
                found = true;
            }

            if (!SectionUtilities.StartingExercisePack(profile))
            {
                isQuestion = true;
                dimension = profile.CurrentState.CourseLocationInfo.TopicLocationInfo.ExerciseSectionState.CurrentQuestionDimension;
            }

            GetNextSentenceResult result = new GetNextSentenceResult(sentence: sentence, success: found, isQuestion: isQuestion, dimension: dimension, isReview: false);

            return result;
        }

        private GetNextSentenceResult HandleReviewSection(UserProfile profile)
        {
            // PRE: We have already been moved to the review section by the state evaluator.
            // select sentence for review.
            // return it.

            Sentence sentence = null;
            bool found = false;

            sentence = FindSentence(profile);

            if (sentence != null)
            {
                found = true;
            }

            GetNextSentenceResult result = new GetNextSentenceResult(sentence: sentence, success: found, isQuestion: true, dimension: QuestionDimension.Understanding, isReview: true);

            return result;
        }

        private Sentence FindSentence(UserProfile profile)
        {
            // TODO: This is where the sentence selection logic goes. 

            Sentence targetSentence = null;

            var currentBatch = SessionManager.Current.GetSessionById(profile).SentenceBatch;

            // Sentence selection logic.
            // 1. Apply all filters.
            // 2. see if its not in history.
            // 3. return it.

            foreach (var sentence in currentBatch.Skip(profile.CurrentState.CurrentIndexWithinBatch))
            {
                bool result = FilterManager.ApplyFilters(profile, sentence);

                if (result)
                {
                    var hi = profile.History.SentenceHistory.Find(shi => shi.Sentence.ObjectId.Equals(sentence.ObjectId));
                    if (hi == null)
                    {
                        targetSentence = sentence;
                        break;
                    }
                }
            }

            // TODO: Reload batches and stuff when we run out.
            // and re-run this logic.

            return targetSentence;
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
