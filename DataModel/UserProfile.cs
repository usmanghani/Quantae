using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class UserProfile : QuantaeObject<long>
    {
        #region User Info

        public string Salt { get; set; }
        public string UserID { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FacebookToken { get; set; }
        public string TwitterToken { get; set; }

        #endregion

        #region Payment Related Info

        public PaymentInformation PaymentInfo { get; set; }

        #endregion

        #region User Session State

        public UserCurrentState CurrentState { get; set; }

        #endregion

        #region User Learning Scores

        /// <summary>
        /// Analytical/Contextual as a continuum.
        /// How to calculate this score:
        /// Using Weighted Moving Average
        /// The formula and weights are in QuantaeEngine.LearningTypeScorePolicies
        /// </summary>
        public LearningTypeScoreModel LearningTypeScore { get; set; }

        /// <summary>
        /// FUTURE: These might be used in the future.
        /// </summary>
        public double DepthScore { get; set; }
        public double LearningDependencyScore { get; set; }
        public double MemoryScore { get; set; }

        #endregion

        #region History Vocab/Verb etc

        /// <summary>
        /// This counter list is updated for each topic seen. This is how it works.
        /// Every time a user sees any topic (any topic new/old), the counters for all
        /// of these topics are updated.  If any one of the counters hit the threshold (2 currently)
        ///  we take the user back to that topic.
        /// </summary>
        public Dictionary<TopicHandle, int> FailureCounters { get; set; }

        public List<TopicHistoryItem> TopicHistory { get; set; }
        public List<SentenceHistoryItem> SentenceHistory { get; set; }
        public List<VocabularyHistoryItem> VocabHistory { get; set; }
        public List<VerbConjugationHistoryItem> VerbConjugationHistory { get; set; }
        public List<NounConjugationHistoryItem> NounConjugationHistory { get; set; }

        #endregion

        #region Weaknesses

        /// <summary>
        /// Count of each weakness per topic
        /// Majority weakness is defined by a threshold.
        /// Understanding is always a major weakness.
        /// </summary>
        public Dictionary<Weakness, int> Weaknesses { get; set; }

        #endregion

        public UserProfile()
        {
            InitNounConjugationHistory();
            InitVerbConjugationHistory();
        }

        private void InitVerbConjugationHistory()
        {
            // TODO : Initialize this properly.
        }

        private void InitNounConjugationHistory()
        {
            NounConjugationHistory = new List<NounConjugationHistoryItem>();
        }
    }

    public class UserProfileHandle : QuantaeObjectHandle<long, UserProfile>
    {
        public UserProfileHandle(UserProfile profile) : base(profile) { }
    }
}
