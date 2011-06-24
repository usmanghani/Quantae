using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class VerbConjugationFilter
    {
        public bool IsSentenceValid(Sentence sentence, UserProfile user)
        {
            var verbConjugations = sentence.RoleConjugationPairs.Where(t => t.Item2.GetType() == typeof(VerbConjugation)).Select(t => t.Item2 as VerbConjugation);
            var userVerbConjugations = user.VerbConjugationHistory.Select(vchi => vchi.VerbConjugation);

            foreach (var vc in verbConjugations)
            {
                if (!userVerbConjugations.Contains(vc))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
