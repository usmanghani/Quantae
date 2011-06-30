using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Quantae.DataModel
{
    public class GrammarEntry : QuantaeObject
    {
        public string Text { get; set; }
        public string Translation { get; set; }
        public WordType WordType { get; set; }
        public Conjugation Conjugation { get; set; }
        public DefinitenessRule DefinitenessRule { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as GrammarEntry).GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode() + Translation.GetHashCode();
        }
    }
}
