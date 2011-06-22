using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class GrammarAnalysisElement
    {
        public Tuple<List<int>, List<Role>> StartSegmentRolePair { get; set; }
        public Tuple<List<int>, List<Role>> EndSegmentRolePair { get; set; }
    }
}
