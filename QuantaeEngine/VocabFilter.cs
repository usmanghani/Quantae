using Quantae.DataModel;

namespace Quantae.Engine
{
    public class VocabFilter : ISentenceFilter
    {
        public bool IsSentenceValid(UserProfile user, Sentence sentence)
        {
            int minSentenceScore = VocabPolicies.CalculateMinSentenceScore(sentence);
            int currentSentenceScore = 0;
            bool result = false;

            foreach (var vocabEntry in sentence.VocabEntries)
            {
                currentSentenceScore += VocabPolicies.GetVocabEntryRank(user, vocabEntry);
            }

            // TODO: ask zeeshan about this comparison function.
            if (currentSentenceScore >= minSentenceScore)
            {
                result = true;
            }

            return result;
        }
    }
}
