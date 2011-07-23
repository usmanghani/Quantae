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

        // FIX: Remove these as they are relevant for topic history item only.

        // FIX: Remove grammar roles and role conjugation pairs because they are not required.

        public List<int> Dependencies { get; set; }
        public List<int> ForwardLinks { get; set; }

        public StaticSection IntroSection { get; set; }

        // TODO: Introduce a depth gateway slide here.

        public StaticSection DepthSection { get; set; }

        public Topic()
        {
            this.Dependencies = new List<int>();
            this.ForwardLinks = new List<int>();
        }
    }

    public class TopicHandle : QuantaeObjectHandle<Topic>
    {
        public TopicHandle(Topic t) : base(t)
        {
        }
    }
}