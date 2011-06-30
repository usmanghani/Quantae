using System.Collections.Generic;

namespace Quantae.DataModel
{
    public class Slide
    {
        public string Content { get; set; }
        public List<VocabEntryHandle> VocabEntries { get; set; }
    }
}