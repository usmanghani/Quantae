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

            sentence = SentenceOperations.FindSentence(profile);

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

            sentence = SentenceOperations.FindSentence(profile);

            if (sentence != null)
            {
                found = true;
            }

            GetNextSentenceResult result = new GetNextSentenceResult(sentence: sentence, success: found, isQuestion: true, dimension: QuestionDimension.Understanding, isReview: true);

            return result;
        }
    }
}
