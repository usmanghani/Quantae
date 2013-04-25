using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class VerbConjugationOperations
    {

        public static void UpdateVerbConjugationHistory(UserProfile profile, VerbConjugation conj, AnswerScore score)
        {
            var vchi = profile.History.VerbConjugationHistory.Find(v => v.VerbConjugation.Equals(conj));

            if (vchi == null)
            {
                vchi = new VerbConjugationHistoryItem() { VerbConjugation = conj };
                profile.History.VerbConjugationHistory.Insert(0, vchi);
            }

            HistoryItemOperations.UpdateHistoryItemWithSuccessFailureAndTimestamp(vchi, score);

        }

        public static void UpdateVerbConjugationHistoryFromSentence(UserProfile profile, Sentence sentence, AnswerScore score)
        {
            var verbConjugations = sentence.RoleConjugationPairs.Where(qt => qt.Item2.GetType() == typeof(VerbConjugation)).Select(qt => qt.Item2 as VerbConjugation);
            foreach (var vc in verbConjugations)
            {
                UpdateVerbConjugationHistory(profile, vc, score);
            }
        }
    }
}
