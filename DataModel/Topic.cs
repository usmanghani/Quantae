using System.Collections.Generic;

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
    public class Topic : QuantaeObject
    {
        /// <summary>
        /// This is used  to give each topic a number. 
        /// This is used to simplify most of our graph walking algorithms.
        /// </summary>
        public int Index { get; set; }
        public string TopicName { get; set; }

        public string AnalyticalTopicTitle { get; set; }
        public string ContextualTopicTitle { get; set; }
        public string AnalyticalLocalizedTopicTitle { get; set; }
        public string ContextualLocalizedTopicTitle { get; set; }

        public List<QuantaeTuple<GrammarRoleHandle, Conjugation>> RoleConjugationPairs { get; set; }

        public bool IsPseudoTopic { get; set; }
        public Weakness WeaknessForPseudoTopic { get; set; }

        public List<TopicHandle> Dependencies { get; set; }
        public List<TopicHandle> ForwardLinks { get; set; }

        public StaticSection IntroSection { get; set; }
        public StaticSection DepthSection { get; set; }

        public Topic()
        {
            this.RoleConjugationPairs = new List<QuantaeTuple<GrammarRoleHandle, Conjugation>>();
            this.Dependencies = new List<TopicHandle>();
            this.ForwardLinks = new List<TopicHandle>();
        }
    }

    public class TopicHandle : QuantaeObjectHandle<Topic>
    {
        public TopicHandle(Topic t) : base(t)
        {
        }
    }
}