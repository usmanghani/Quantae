 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class NounConjugationHistoryItem : HistoryItem
    {
        public NounConjugation NounConjugation { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as NounConjugationHistoryItem).NounConjugation.Equals(NounConjugation);
        }

        public override int GetHashCode()
        {
            return NounConjugation.GetHashCode();
        }
    }
}
