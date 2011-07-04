using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class StaticPage : Page
    {
        public string Content { get; set; }
        public List<VocabEntryHandle> VocabEntries { get; set; }
    }
}
