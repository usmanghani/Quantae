using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class SentenceHistoryItem : HistoryItem
    {
        public SentenceHandle Sentence { get; set; }
        public SentenceFragment Intent { get; set; }
    }
}
