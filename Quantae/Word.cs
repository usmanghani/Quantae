using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Quantae
{
    public class Word : QuantaeObject<ulong>
    {
        public string Text { get; set; }
        public string Translation { get; set; }
        public WordType WordType { get; set; }
        public WordSubtype WordSubtype { get; set; }
        public NounConjugation NounConjugation { get; set; }
        public VerbConjugation VerbConjugation { get; set; }
        public DefinitenessRule DefinitnessRule { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as Word).Text == Text;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }
    }
}
