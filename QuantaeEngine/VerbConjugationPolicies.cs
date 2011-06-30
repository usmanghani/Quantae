using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class VerbConjugationPolicies
    {
        private const double PassPercentage = 0.9;
        private const int MinSuccessRequired = 9;

        private static Dictionary<int, Tuple<VerbConjugation, VerbConjugation>> pastTenseConjugationRankingMap = new Dictionary<int, Tuple<VerbConjugation, VerbConjugation>>{
            {0, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Singular, Gender = GenderRule.Masculine, Person = PersonRule.Third},  new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Singular, Gender = GenderRule.Feminine, Person = PersonRule.Third})},
            {1, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Singular, Gender = GenderRule.Neutral, Person = PersonRule.First},    new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Plural, Gender = GenderRule.Neutral, Person = PersonRule.First})},
            {2, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Singular, Gender = GenderRule.Masculine, Person = PersonRule.Second}, new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Singular, Gender = GenderRule.Feminine, Person = PersonRule.Second})},
            {3, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Plural, Gender = GenderRule.Masculine, Person = PersonRule.Second},   new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Plural, Gender = GenderRule.Masculine, Person = PersonRule.Third})},
            {4, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Dual, Gender = GenderRule.Masculine, Person = PersonRule.Third},      new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Dual, Gender = GenderRule.Feminine, Person = PersonRule.Third})},
            {5, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Dual, Gender = GenderRule.Masculine, Person = PersonRule.Second},     new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Dual, Gender = GenderRule.Feminine, Person = PersonRule.Second})},
            {6, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Plural, Gender = GenderRule.Feminine, Person = PersonRule.Second},    new VerbConjugation(){Tense = TenseRule.Past, Number = NumberRule.Plural, Gender = GenderRule.Feminine, Person = PersonRule.Third})}
        };

        private static Dictionary<int, Tuple<VerbConjugation, VerbConjugation>> presentFutureTenseConjugationRankingMap = new Dictionary<int, Tuple<VerbConjugation, VerbConjugation>>{
            {0, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Singular, Gender = GenderRule.Masculine, Person = PersonRule.Third},     new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Singular, Gender = GenderRule.Feminine, Person = PersonRule.Third})},
            {1, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Singular, Gender = GenderRule.Neutral, Person = PersonRule.First},       new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Plural, Gender = GenderRule.Neutral, Person = PersonRule.First})},
            {2, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Singular, Gender = GenderRule.Masculine, Person = PersonRule.Second},    new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Singular, Gender = GenderRule.Feminine, Person = PersonRule.Second})},
            {3, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Plural, Gender = GenderRule.Masculine, Person = PersonRule.Second},      new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Plural, Gender = GenderRule.Masculine, Person = PersonRule.Third})},
            {4, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Dual, Gender = GenderRule.Masculine, Person = PersonRule.Third},         new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Dual, Gender = GenderRule.Feminine, Person = PersonRule.Third})},
            {5, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Dual, Gender = GenderRule.Masculine, Person = PersonRule.Second},        new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Dual, Gender = GenderRule.Feminine, Person = PersonRule.Second})},
            {6, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Plural, Gender = GenderRule.Feminine, Person = PersonRule.Second},       new VerbConjugation(){Tense = TenseRule.PresentFuture, Number = NumberRule.Plural, Gender = GenderRule.Feminine, Person = PersonRule.Third})}
        };

        private static Dictionary<int, Tuple<VerbConjugation, VerbConjugation>> commandTenseConjugationRankingMap = new Dictionary<int, Tuple<VerbConjugation, VerbConjugation>>{
            {0, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.Command, Number = NumberRule.Singular, Gender = GenderRule.Masculine, Person = PersonRule.Second},  new VerbConjugation(){Tense = TenseRule.Command, Number = NumberRule.Singular, Gender = GenderRule.Feminine, Person = PersonRule.Second})},
            {1, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.Command, Number = NumberRule.Plural, Gender = GenderRule.Masculine, Person = PersonRule.Second},    new VerbConjugation(){Tense = TenseRule.Command, Number = NumberRule.Plural, Gender = GenderRule.Feminine, Person = PersonRule.Second})},
            {2, new Tuple<VerbConjugation, VerbConjugation>(new VerbConjugation(){Tense = TenseRule.Command, Number = NumberRule.Dual, Gender = GenderRule.Masculine, Person = PersonRule.Second},      new VerbConjugation(){Tense = TenseRule.Command, Number = NumberRule.Dual, Gender = GenderRule.Feminine, Person = PersonRule.Second})}
        };

        public static IDictionary<TenseRule, List<VerbConjugationHistoryItem>> GetCurrentVerbConjugations(UserProfile user)
        {
            Dictionary<TenseRule, Tuple<VerbConjugation, VerbConjugation>> verbConjugationsToFind = new Dictionary<TenseRule, Tuple<VerbConjugation, VerbConjugation>>();

            verbConjugationsToFind[TenseRule.Past] = pastTenseConjugationRankingMap[user.CurrentState.CurrentVerbConjugationRanksByTense[TenseRule.Past]];
            verbConjugationsToFind[TenseRule.PresentFuture] = presentFutureTenseConjugationRankingMap[user.CurrentState.CurrentVerbConjugationRanksByTense[TenseRule.PresentFuture]];
            verbConjugationsToFind[TenseRule.Command] = commandTenseConjugationRankingMap[user.CurrentState.CurrentVerbConjugationRanksByTense[TenseRule.Command]];

            var verbConjugationsFound = new Dictionary<TenseRule, List<VerbConjugationHistoryItem>>();

            foreach (var key in verbConjugationsToFind.Keys)
            {
                foreach (var vchi in user.VerbConjugationHistory)
                {
                    if ((vchi.VerbConjugation == verbConjugationsToFind[key].Item1) || (vchi.VerbConjugation == verbConjugationsToFind[key].Item2))
                    {
                        if (verbConjugationsFound[key] == null)
                        {
                            verbConjugationsFound[key] = new List<VerbConjugationHistoryItem>();
                        }

                        verbConjugationsFound[key].Add(vchi);
                    }
                }
            }

            return verbConjugationsFound;
        }

        public static bool CanMoveToNextVerbConjugation(UserProfile user, TenseRule tenseRule)
        {
            var currentVerbConjugations = GetCurrentVerbConjugations(user);

            if (currentVerbConjugations[tenseRule].Count < 2)
            {
                return false;
            }

            foreach (var vchi in currentVerbConjugations[tenseRule])
            {

                if (!IsVerbConjugationSuccessful(vchi))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsVerbConjugationSuccessful(VerbConjugationHistoryItem vchi)
        {
            if (vchi.SuccessCount < MinSuccessRequired)
            {
                return false;
            }

            if ((vchi.FailureCount + vchi.SuccessCount) <= 0)
            {
                return false;
            }

            return (vchi.SuccessCount / (vchi.FailureCount + vchi.SuccessCount)) >= PassPercentage;
        }
    }
}
