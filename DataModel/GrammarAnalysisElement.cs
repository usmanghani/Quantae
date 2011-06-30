using System.Collections.Generic;

namespace Quantae.DataModel
{
    public class GrammarAnalysisElement
    {
        public QuantaeTuple<HashSet<int>, GrammarRoleHandle> StartSegmentRolePair { get; set; }
        public QuantaeTuple<HashSet<int>, GrammarRoleHandle> EndSegmentRolePair { get; set; }
    }
}