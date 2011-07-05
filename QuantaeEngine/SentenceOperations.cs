using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class SentenceOperations
    {
        private static int BatchSize = 50;

        internal static Sentence FindSentence(UserProfile profile)
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

        public static Sentence GetSentenceFromHandle(SentenceHandle sentenceHandle)
        {
            return Repositories.Repositories.Sentences.GetItemByHandle(sentenceHandle);
        }

        public static List<Sentence> LoadBatch(UserProfile profile)
        {
            // TODO: Figure out what happens when the topic is pseudo topic.
            // In that case we can't load by primary topic. We have to load by
            // secondary topics (Either conjugations or tags)

            var sentences = Repositories.Repositories.Sentences.GetSentencesByTopic(
                profile.CurrentState.CourseLocationInfo.CurrentTopic.Topic,
                profile.CurrentState.CurrentBatchIndex * BatchSize,
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
