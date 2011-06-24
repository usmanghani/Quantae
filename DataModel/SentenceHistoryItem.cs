using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class SentenceHistoryItem : HistoryItem
    {
        public SentenceHandle Sentence { get; set; }
        public SentenceFragment Intent { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as SentenceHistoryItem).Sentence.Equals(Sentence);
        }

        public override int GetHashCode()
        {
            return Sentence.GetHashCode();
        }
    }
}
