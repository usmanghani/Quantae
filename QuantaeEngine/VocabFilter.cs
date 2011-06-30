using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class VocabFilter : ISentenceFilter
    {
        public bool IsSentenceValid(UserProfile user, Sentence sentence)
        {
            int minSentenceScore = VocabPolicies.CalculateMinSentenceScore(sentence);

            int currentSentenceScore = 0;
            foreach (var vocabEntry in sentence.VocabEntries)
            {
                currentSentenceScore += VocabPolicies.GetVocabEntryRank(user, vocabEntry);
            }

            // TODO: ask zeeshan about this comparison function.
            if (currentSentenceScore >= minSentenceScore)
            {
                return true;
            }

            return false;
        }
    }
}
