using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;
using Quantae.Repositories;
using MongoDB.Driver.Builders;

namespace Quantae.Engine
{
    public class SentenceOperations
    {
        private static int BatchSize = 50;

        public static Sentence FindSentence(UserProfile profile)
        {
            // TODO: This is where the sentence selection logic goes. 

            Sentence targetSentence = null;

            //var currentBatch = SessionManager.Current.GetSessionById(profile).SentenceBatch;

            // Sentence selection logic.
            // 1. Apply all filters.
            // 2. see if its not in history.
            // 3. return it.

            //var sentences = Repositories.Repositories.Sentences.GetAllItems();
            var sentences = Repositories.Repositories.Sentences.FindAs(SentenceQueries.GetSentencesByTopic(profile.CurrentState.CourseLocationInfo.CurrentTopic.Topic));
            foreach (var sentence in sentences)
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

            //foreach (var sentence in currentBatch.Skip(profile.CurrentState.CurrentIndexWithinBatch))
            //{
            //    bool result = FilterManager.ApplyFilters(profile, sentence);

            //    if (result)
            //    {
            //        var hi = profile.History.SentenceHistory.Find(shi => shi.Sentence.ObjectId.Equals(sentence.ObjectId));
            //        if (hi == null)
            //        {
            //            targetSentence = sentence;
            //            break;
            //        }
            //    }
            //}

            // TODO: Reload batches and stuff when we run out.
            // and re-run this logic.

            return targetSentence;
        }

        // Make a UserProfileManager and move this there along with other update functionality related to the user.
        public static void UpdateUserProfileWithCurrentSentenceResponse(UserProfile profile, AnswerDimension answerDimension, AnswerScore score)
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

        public static Sentence GetSentenceFromHandle(SentenceHandle sentenceHandle)
        {
            return Repositories.Repositories.Sentences.GetItemByHandle(sentenceHandle);
        }

        public static List<Sentence> LoadBatch(UserProfile profile)
        {
            // TODO: Figure out what happens when the topic is pseudo topic.
            // In that case we can't load by primary topic. We have to load by
            // secondary topics (Either conjugations or tags)

            var sentences = Repositories.Repositories.Sentences.FindAs(SentenceQueries.GetSentencesByTopic(
                profile.CurrentState.CourseLocationInfo.CurrentTopic.Topic), 
                SortBy.Null,
                profile.CurrentState.CurrentBatchIndex * BatchSize,
                BatchSize,
                BatchSize);

            profile.CurrentState.CurrentIndexWithinBatch = 0;

            return sentences.ToList();
        }

        public static List<Sentence> LoadBatch(UserProfileHandle profileHandle)
        {
            return LoadBatch(Repositories.Repositories.Users.GetItemByHandle(profileHandle));
        }
    }
}
