using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class GrammarAnalysisElement
    {
        public List<int> Start { get; set; }
        public List<int> End { get; set; }

        public TopicComponent StartComponent { get; set; }
        public TopicComponent EndComponent { get; set; }
    }
}
