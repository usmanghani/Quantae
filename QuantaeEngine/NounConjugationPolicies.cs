using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class NounConjugationPolicies
    {
        private const double PassPercentage = 0.9;
        private const int MinSuccessRequired = 9;

        private static Dictionary<int, Tuple<NounConjugation, NounConjugation>> nounConjugationRankingMap = new Dictionary<int, Tuple<NounConjugation, NounConjugation>>{
            {0, new Tuple<NounConjugation, NounConjugation>(new NounConjugation(){Number = NumberRule.Singular, Gender = GenderRule.Masculine}, new NounConjugation(){Number = NumberRule.Singular, Gender = GenderRule.Feminine})},
            {1, new Tuple<NounConjugation, NounConjugation>(new NounConjugation(){Number = NumberRule.Plural, Gender = GenderRule.Masculine}, new NounConjugation(){Number = NumberRule.Plural, Gender = GenderRule.Feminine})},
            {2, new Tuple<NounConjugation, NounConjugation>(new NounConjugation(){Number = NumberRule.Dual, Gender = GenderRule.Masculine}, new NounConjugation(){Number = NumberRule.Dual, Gender = GenderRule.Feminine})}
        };

        /// <summary>
        /// Returns the noun conjugations that are at the current rank. Nothing lower or above.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public static IEnumerable<NounConjugationHistoryItem> GetCurrentNounConjugations(UserProfile user)
        {
            var nounConjugationsToFind = nounConjugationRankingMap[user.CurrentState.CurrentNounConjugationRank];

            var nounConjugationsFound = new List<NounConjugationHistoryItem>();

            foreach (var nchi in user.History.NounConjugationHistory)
            {
                if ((nchi.NounConjugation.Equals(nounConjugationsToFind.Item1)) || (nchi.NounConjugation.Equals(nounConjugationsToFind.Item2)))
                {
                    nounConjugationsFound.Add(nchi);
                }
            }

            return nounConjugationsFound;
        }

        public static bool CanMoveToNextNounConjugation(UserProfile user)
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

        public static bool IsNounConjugationSuccessful(NounConjugationHistoryItem nchi)
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

        public static bool DoesTheUserKnowNounConjugation(UserProfile profile, NounConjugation conj)
        {
            foreach (var nchi in profile.History.NounConjugationHistory)
            {
                if (!nchi.NounConjugation.Equals(conj))
                {
                    continue;
                }

                if (IsNounConjugationSuccessful(nchi))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
