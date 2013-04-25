using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class NounConjugationOperations
    {
        public static void UpdateNounConjugationHistory(UserProfile profile, NounConjugation conj, AnswerScore score)
        {
            var nchi = profile.History.NounConjugationHistory.Find(n => n.NounConjugation.Equals(conj));

            if (nchi == null)
            {
                nchi = new NounConjugationHistoryItem() { NounConjugation = conj };
                profile.History.NounConjugationHistory.Insert(0, nchi);
            }

            HistoryItemOperations.UpdateHistoryItemWithSuccessFailureAndTimestamp(nchi, score);
        }

        public static void UpdateNounConjugationHistoryFromSentence(UserProfile profile, Sentence sentence, AnswerScore score)
        {
            var nounConjugations = sentence.RoleConjugationPairs.Where(qt => qt.Item2.GetType() == typeof(NounConjugation)).Select(qt => qt.Item2 as NounConjugation);
            foreach (var nc in nounConjugations)
            {
                UpdateNounConjugationHistory(profile, nc, score);
            }
        }
    }
}
