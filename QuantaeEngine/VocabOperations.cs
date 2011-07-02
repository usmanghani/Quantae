using System.Collections.Generic;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class VocabOperations
    {
        public static void UpdateVocabulary(UserProfile userProfile, List<VocabEntryHandle> entries, VocabRankTypes rankType, AnswerScore score = AnswerScore.Unknown)
        {
            foreach (var entry in entries)
            {
                UpdateVocabulary(userProfile, entry, rankType, score);
            }
        }

        public static void UpdateVocabulary(UserProfile userProfile, VocabEntryHandle entry, VocabRankTypes rankType, AnswerScore score = AnswerScore.Unknown)
        {
            VocabularyHistoryItem vhi = userProfile.VocabHistory.Find(h => h.VocabEntry.Equals(entry));

            if (vhi == null)
            {
                vhi = new VocabularyHistoryItem() { Rank = rankType, VocabEntry = entry };
                userProfile.VocabHistory.Insert(0, vhi);
            }

            HistoryItemOperations.UpdateHistoryItemWithSuccessFailureAndTimestamp(vhi, score);
        }
    }
}
