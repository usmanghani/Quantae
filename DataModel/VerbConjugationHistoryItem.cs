using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class VerbConjugationHistoryItem : HistoryItem
    {
        public VerbConjugation VerbConjugation { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as VerbConjugationHistoryItem).VerbConjugation.Equals(VerbConjugation);
        }

        public override int GetHashCode()
        {
            return VerbConjugation.GetHashCode();
        }
    }
}
