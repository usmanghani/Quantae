using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class VocabPolicies
    {
        private double vocabEntryWeight = 1.5;

        public int CalculateMinSentenceScore(Sentence sentence)
        {
            return (int)Math.Ceiling(sentence.VocabEntries.Count * 1.5);
        }

        public int GetVocabEntryRank(VocabEntryHandle handle, UserProfile user)
        {
            var historyItem = user.VocabHistory.Where(vhi => vhi.VocabEntry.Equals(handle)).FirstOrDefault();
            if (historyItem == null)
            {
                return 0;
            }

            return (int)historyItem.Rank;
        }
    }
}
