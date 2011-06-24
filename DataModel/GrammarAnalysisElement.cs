using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class GrammarAnalysisElement
    {
        public Tuple<HashSet<int>, GrammarRoleHandle> StartSegmentRolePair { get; set; }
        public Tuple<HashSet<int>, GrammarRoleHandle> EndSegmentRolePair { get; set; }
    }
}
