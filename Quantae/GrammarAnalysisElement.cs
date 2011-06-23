using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class GrammarAnalysisElement
    {
        public Tuple<HashSet<int>, Role> StartSegmentRolePair { get; set; }
        public Tuple<HashSet<int>, Role> EndSegmentRolePair { get; set; }
    }
}
