using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class NounConjugationFilter : ISentenceFilter
    {
        public bool IsSentenceValid(UserProfile user, Sentence sentence)
        {
            var nounConjugations = sentence.RoleConjugationPairs.Where(t => t.Item2.GetType() == typeof(NounConjugation)).Select(t => t.Item2 as NounConjugation);
            var userNounConjugations = user.NounConjugationHistory.Select(nchi => nchi.NounConjugation);

            foreach (var nc in nounConjugations)
            {
                if (!userNounConjugations.Contains(nc))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
