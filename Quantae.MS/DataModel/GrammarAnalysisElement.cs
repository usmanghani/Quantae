using System.Collections.Generic;

namespace Quantae.DataModel
{
    public class GrammarAnalysisElement
    {
        public QuantaeTuple<List<int>, GrammarRoleHandle> StartSegmentRolePair { get; set; }
        public QuantaeTuple<List<int>, GrammarRoleHandle> EndSegmentRolePair { get; set; }
    }
}