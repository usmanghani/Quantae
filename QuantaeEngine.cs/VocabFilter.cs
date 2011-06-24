using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class VocabFilter
    {
        public bool IsSentenceValid(Sentence sentence, UserProfile user)
        {
            VocabPolicies policies = new VocabPolicies();

            int minSentenceScore = policies.CalculateMinSentenceScore(sentence);

            int currentSentenceScore = 0;
            foreach (var vocabEntry in sentence.VocabEntries)
            {
                currentSentenceScore += policies.GetVocabEntryRank(vocabEntry, user);
            }

            if (currentSentenceScore >= minSentenceScore)
            {
                return true;
            }

            return false;
        }
    }
}
