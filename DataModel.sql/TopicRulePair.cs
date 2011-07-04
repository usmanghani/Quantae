using System.Collections.Generic;

namespace Quantae.DataModel.Sql
{
    public class TopicRulePair
    {
        public Topic Topic { get; set; }
        public List<NounConjugation> NounConjugation { get; set; }
        public List<VerbConjugation> VerbConjugation { get; set; }
    }
}