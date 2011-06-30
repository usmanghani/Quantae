using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class VocabOperations
    {
        public static void UpdateVocabulary(UserProfile userProfile, List<VocabEntryHandle> entries, VocabRankTypes rankType, Tristate success = Tristate.DontCare)
        {
            foreach (var entry in entries)
            {
                UpdateVocabulary(userProfile, entry, rankType, success);
            }
        }

        public static void UpdateVocabulary(UserProfile userProfile, VocabEntryHandle entry, VocabRankTypes rankType, Tristate success = Tristate.DontCare)
        {
            VocabularyHistoryItem vhi = new VocabularyHistoryItem() { Rank = rankType, VocabEntry = entry, LastTimestamp = DateTime.UtcNow };

            if (success == Tristate.True)
            {
                vhi.SuccessCount++;
            }
            
            if (success == Tristate.False)
            {
                vhi.FailureCount++;
            }

            userProfile.VocabHistory.Add(vhi);
        }
    }
}
