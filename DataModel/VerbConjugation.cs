using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class VerbConjugation : Conjugation
    {
        public PersonRule Person { get; set; }
        public TenseRule Tense { get; set; }

        public override bool Equals(object obj)
        {
            VerbConjugation item = obj as VerbConjugation;

            return (item.Gender == Gender)
                && (item.Number == Number)
                && (item.Person == Person)
                && (item.Tense == Tense);
        }

        public override int GetHashCode()
        {
            return (Gender.ToString() + Number.ToString() + Person.ToString() + Tense.ToString()).GetHashCode();
        }
    }
}
