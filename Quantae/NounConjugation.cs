using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class NounConjugation
    {
        public GenderRule Gender { get; set; }
        public NumberRule Number { get; set; }

        public override bool Equals(object obj)
        {
            NounConjugation item = obj as NounConjugation;
            return (item.Gender == Gender) && (item.Number == Number);
        }

        public override int GetHashCode()
        {
            return (Gender.ToString() + Number.ToString()).GetHashCode();
        }
    }
}
