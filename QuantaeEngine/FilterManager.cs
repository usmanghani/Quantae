using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class FilterManager
    {
        private static Dictionary<int, ISentenceFilter> sentenceFilters = new Dictionary<int, ISentenceFilter>();

        public static void CreateFilters()
        {
            //sentenceFilters.Add(0, new PrimaryTopicFilter());
            //sentenceFilters.Add(1, new SecondaryTopicFilter());
            //sentenceFilters.Add(2, new VerbConjugationFilter());
            //sentenceFilters.Add(3, new NounConjugationFilter());
            //sentenceFilters.Add(4, new VocabFilter());
            //sentenceFilters.Add(5, new PseudoTopicFilter());
        }

        public static bool ApplyFilters(UserProfile profile, Sentence sentence)
        {
            bool result = true;
            foreach (var i in sentenceFilters.Keys)
            {
                if (!sentenceFilters[i].IsSentenceValid(profile, sentence))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}
