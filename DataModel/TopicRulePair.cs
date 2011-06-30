using System.Collections.Generic;

namespace Quantae.DataModel
{
    public class TopicRulePair
    {
        public Topic Topic { get; set; }
        public List<NounConjugation> NounConjugation { get; set; }
        public List<VerbConjugation> VerbConjugation { get; set; }
    }
}