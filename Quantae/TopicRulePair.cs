using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Quantae
{
    public class TopicRulePair
    {
        public Topic Topic { get; set; }
        public List<NounConjugation> NounConjugation { get; set; }
        public List<VerbConjugation> VerbConjugation { get; set; }
    }
}
