using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace Quantae
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
        public string LocalizedTopicName { get; set; }
        public List<TopicComponent> TopicComponents { get; set; }
        public bool IsPseudoTopic { get; set; }
        public List<Topic> Dependencies { get; set; }
        public IntroSection IntroSection { get; set; }
    }
}
