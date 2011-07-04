using System.Collections.Generic;

namespace Quantae.DataModel.Sql
{
    public class UserProfile : QuantaeObject
    {
        #region User Info

        public string UserID { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FacebookToken { get; set; }
        public string TwitterToken { get; set; }

        #endregion

        #region Payment Related Info

        public PaymentInformation PaymentInfo { get; set; }

        #endregion

        #region User Current State

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
        public DepthScoreModel DepthScore { get; set; }
        public DependencyScoreModel LearningDependencyScore { get; set; }
        public MemoryScoreModel MemoryScore { get; set; }

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
            this.NounConjugationHistory = new List<NounConjugationHistoryItem>();
            this.VerbConjugationHistory = new List<VerbConjugationHistoryItem>();
            this.SentenceHistory = new List<SentenceHistoryItem>();
            this.TopicHistory = new List<TopicHistoryItem>();
            this.VocabHistory = new List<VocabularyHistoryItem>();
            this.Weaknesses = new Dictionary<Weakness, int>();
            this.FailureCounters = new Dictionary<TopicHandle, int>();

            this.LearningTypeScore = new LearningTypeScoreModel();
            this.DepthScore = new DepthScoreModel();
            this.LearningDependencyScore = new DependencyScoreModel();
            this.MemoryScore = new MemoryScoreModel();

            this.CurrentState = new UserCurrentState();
        }
    }

    public class UserProfileHandle : QuantaeObjectHandle<UserProfile>
    {
        public UserProfileHandle(UserProfile profile)
            : base(profile)
        {
        }
    }
}