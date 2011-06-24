using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace Quantae.DataModel
{
    /// <summary>
    /// These are graph nodes
    /// 
    /// Mudaf-MudafIlaihi (Possessive Phrase)
    /// Ism Mosool Jumla Sila (Relative Pronoun Phrase)
    /// Ism Mosool Jumla Sila Wa Al Aaid (Relative and Personal Pronoun Phrase)
    /// 
    /// </summary>
    public class Topic : QuantaeObject<ulong>
    {
        public string TopicName { get; set; }

        public string AnalyticalTopicTitle { get; set; }
        public string ContextualTopicTitle { get; set; }
        public string AnalyticalLocalizedTopicTitle { get; set; }
        public string ContextualLocalizedTopicTitle { get; set; }

        public List<Tuple<GrammarRoleHandle, Conjugation>> RoleConjugationPairs { get; set; }

        public bool IsPseudoTopic { get; set; }

        public List<TopicHandle> Dependencies { get; set; }
        public List<TopicHandle> ForwardLinks { get; set; }

        public IntroSection IntroSection { get; set; }
    }

    public class TopicHandle : QuantaeObjectHandle<ulong>
    {

    }
}
