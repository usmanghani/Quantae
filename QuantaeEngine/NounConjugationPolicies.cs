using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class NounConjugationPolicies
    {
        private double PassPercentage = 0.9;
        private int MinSuccessRequired = 9;

        private Dictionary<int, Tuple<NounConjugation, NounConjugation>> nounConjugationRankingMap = new Dictionary<int, Tuple<NounConjugation, NounConjugation>>{
            {0, new Tuple<NounConjugation, NounConjugation>(new NounConjugation(){Number = NumberRule.Singular, Gender = GenderRule.Masculine}, new NounConjugation(){Number = NumberRule.Singular, Gender = GenderRule.Feminine})},
            {1, new Tuple<NounConjugation, NounConjugation>(new NounConjugation(){Number = NumberRule.Plural, Gender = GenderRule.Masculine}, new NounConjugation(){Number = NumberRule.Plural, Gender = GenderRule.Feminine})},
            {2, new Tuple<NounConjugation, NounConjugation>(new NounConjugation(){Number = NumberRule.Dual, Gender = GenderRule.Masculine}, new NounConjugation(){Number = NumberRule.Dual, Gender = GenderRule.Feminine})}
        };

        public IEnumerable<NounConjugationHistoryItem> GetCurrentNounConjugations(UserProfile user)
        {
            var nounConjugationsToFind = nounConjugationRankingMap[user.CurrentState.CurrentNounConjugationRank];

            var nounConjugationsFound = new List<NounConjugationHistoryItem>();

            foreach (var nchi in user.NounConjugationHistory)
            {
                if ((nchi.NounConjugation == nounConjugationsToFind.Item1) || (nchi.NounConjugation == nounConjugationsToFind.Item2))
                {
                    nounConjugationsFound.Add(nchi);
                }
            }

            return nounConjugationsFound;
        }

        public bool CanMoveToNextNounConjugation(UserProfile user)
        {
            var currentNounConjugations = GetCurrentNounConjugations(user);

            if (currentNounConjugations.Count() < 2)
            {
                return false;
            }

            foreach (var nchi in currentNounConjugations)
            {

                if (!IsNounConjugationSuccessful(nchi))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsNounConjugationSuccessful(NounConjugationHistoryItem nchi)
        {
            if (nchi.SuccessCount < MinSuccessRequired)
            {
                return false;
            }

            if ((nchi.FailureCount + nchi.SuccessCount) <= 0)
            {
                return false;
            }

            return (nchi.SuccessCount / (nchi.FailureCount + nchi.SuccessCount)) >= PassPercentage;
        }
    }
}
