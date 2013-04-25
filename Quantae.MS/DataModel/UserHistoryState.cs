using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class UserHistoryState
    {
        /// <summary>
        /// This counter list is updated for each topic seen. This is how it works.
        /// Every time a user sees any topic (any topic new/old), the counters for all
        /// of these topics are updated.  If any one of the counters hit the threshold (2 currently)
        ///  we take the user back to that topic.
        /// </summary>
        public Dictionary<TopicHandle, int> FailureCounters { get; set; }

        /// <summary>
        /// Gets or sets the topic history.
        /// </summary>
        /// <value>
        /// The topic history.
        /// </value>
        public List<TopicHistoryItem> TopicHistory { get; set; }

        /// <summary>
        /// Gets or sets the sentence history.
        /// </summary>
        /// <value>
        /// The sentence history.
        /// </value>
        public List<SentenceHistoryItem> SentenceHistory { get; set; }

        /// <summary>
        /// Gets or sets the vocab history.
        /// </summary>
        /// <value>
        /// The vocab history.
        /// </value>
        public List<VocabularyHistoryItem> VocabHistory { get; set; }

        /// <summary>
        /// Gets or sets the verb conjugation history.
        /// </summary>
        /// <value>
        /// The verb conjugation history.
        /// </value>
        public List<VerbConjugationHistoryItem> VerbConjugationHistory { get; set; }

        /// <summary>
        /// Gets or sets the noun conjugation history.
        /// </summary>
        /// <value>
        /// The noun conjugation history.
        /// </value>
        public List<NounConjugationHistoryItem> NounConjugationHistory { get; set; }

        public UserHistoryState()
        {
            this.NounConjugationHistory = new List<NounConjugationHistoryItem>();
            this.VerbConjugationHistory = new List<VerbConjugationHistoryItem>();
            this.SentenceHistory = new List<SentenceHistoryItem>();
            this.TopicHistory = new List<TopicHistoryItem>();
            this.VocabHistory = new List<VocabularyHistoryItem>();
            this.FailureCounters = new Dictionary<TopicHandle, int>();
        }
    }
}
