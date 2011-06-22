using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class VerbConjugation
    {
        public GenderRule Gender { get; set; }
        public NumberRule Number { get; set; }
        public PersonRule Person { get; set; }
        public TenseRule Tense { get; set; }

        public override bool Equals(object obj)
        {
            VerbConjugation item = obj as VerbConjugation;
            return Gender == item.Gender && Number == item.Number && Person == item.Person && Tense == item.Tense;
        }

        public override int GetHashCode()
        {
            return (Gender.ToString() + Number.ToString() + Person.ToString() + Tense.ToString()).GetHashCode();
        }
    }
}
