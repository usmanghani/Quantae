using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            VocabularyHistoryItem vhi = new VocabularyHistoryItem() { Rank = rankType, VocabEntry = entry };
            HistoryItemOperations.UpdateHistoryItemWithSuccessFailureAndTimestamp(vhi, score);
            userProfile.VocabHistory.Insert(0, vhi);
        }
    }
}
