﻿using System;
using System.Linq;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class VocabPolicies
    {
        private static double vocabEntryWeight = 1.5;

        public static int CalculateMinSentenceScore(Sentence sentence)
        {
            return (int)Math.Ceiling(sentence.VocabEntries.Count * vocabEntryWeight);
        }

        public static int GetVocabEntryRank(UserProfile user, VocabEntryHandle handle)
        {
            var historyItem = user.History.VocabHistory.Where(vhi => vhi.VocabEntry.Equals(handle)).FirstOrDefault();
            if (historyItem == null)
            {
                return 0;
            }

            return (int)historyItem.Rank;
        }
    }
}
