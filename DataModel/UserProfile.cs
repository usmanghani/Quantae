using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class UserProfile : QuantaeObject<ulong>
    {
        #region User Info

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
        /// </summary>
        public double LearningTypeScore { get; set; }

        /// <summary>
        /// FUTURE:
        /// </summary>
        public double DepthScore { get; set; }
        public double LearningDependencyScore { get; set; }
        public double MemoryScore { get; set; }

        #endregion

        #region History Vocab/Verb etc

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
    }
}
