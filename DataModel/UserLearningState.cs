using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class UserLearningState
    {
        /// <summary>
        /// Analytical/Contextual as a continuum.
        /// How to calculate this score:
        /// Using Weighted Moving Average
        /// The formula and weights are in QuantaeEngine.LearningTypeScorePolicies
        /// </summary>
        public LearningTypeScoreModel LearningTypeScore { get; set; }

        // FUTURE: These might be used in the future.

        /// <summary>
        /// Gets or sets the depth score.
        /// </summary>
        /// <value>
        /// The depth score.
        /// </value>
        public DepthScoreModel DepthScore { get; set; }

        /// <summary>
        /// Gets or sets the learning dependency score.
        /// </summary>
        /// <value>
        /// The learning dependency score.
        /// </value>
        public DependencyScoreModel LearningDependencyScore { get; set; }

        /// <summary>
        /// Gets or sets the memory score.
        /// </summary>
        /// <value>
        /// The memory score.
        /// </value>
        public MemoryScoreModel MemoryScore { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLearningState"/> class.
        /// </summary>
        public UserLearningState()
        {
            this.LearningTypeScore = new LearningTypeScoreModel();
            this.DepthScore = new DepthScoreModel();
            this.LearningDependencyScore = new DependencyScoreModel();
            this.MemoryScore = new MemoryScoreModel();
        }
    }
}
