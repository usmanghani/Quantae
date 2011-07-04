using System.Collections.Generic;

namespace Quantae.DataModel
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

        /// <summary>
        /// Gets or sets the state of the current.
        /// </summary>
        /// <value>
        /// The state of the current.
        /// </value>
        public UserCurrentState CurrentState { get; set; }

        #endregion

        #region User Learning State

        /// <summary>
        /// Gets or sets the learning.
        /// </summary>
        /// <value>
        /// The learning.
        /// </value>
        public UserLearningState Learning { get; set; }

        #endregion

        #region User History State

        /// <summary>
        /// Gets or sets the history.
        /// </summary>
        /// <value>
        /// The history.
        /// </value>
        public UserHistoryState History { get; set; }

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
            this.History = new UserHistoryState();
            this.Learning = new UserLearningState();
            this.CurrentState = new UserCurrentState();

            this.Weaknesses = new Dictionary<Weakness, int>();
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