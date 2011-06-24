using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class NounConjugation : Conjugation
    {
        public override bool Equals(object obj)
        {
            NounConjugation item = obj as NounConjugation;

            return item.Gender == this.Gender && item.Number == this.Number;
        }

        public override int GetHashCode()
        {
            return (Gender.ToString() + Number.ToString()).GetHashCode();
        }
    }
}
