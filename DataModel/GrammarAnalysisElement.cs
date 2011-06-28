using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class GrammarAnalysisElement
    {
        public QuantaeTuple<HashSet<int>, GrammarRoleHandle> StartSegmentRolePair { get; set; }
        public QuantaeTuple<HashSet<int>, GrammarRoleHandle> EndSegmentRolePair { get; set; }
    }
}
