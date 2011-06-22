using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class NounConjugationHistoryItem : HistoryItem
    {
        public NounConjugation NounConjugation { get; set; }

        public override bool Equals(object obj)
        {
            NounConjugation item = obj as NounConjugation;
            return item.Equals(NounConjugation);
        }

        public override int GetHashCode()
        {
            return NounConjugation.GetHashCode();
        }
    }
}
