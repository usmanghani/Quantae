using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class VerbConjugationHistoryItem : HistoryItem
    {
        public VerbConjugation VerbConjugation { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as VerbConjugation).Equals(VerbConjugation);
        }

        public override int GetHashCode()
        {
            return VerbConjugation.GetHashCode();
        }
    }
}
