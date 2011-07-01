using System.Collections.Generic;

namespace Quantae.DataModel
{
    public class TopicHistoryItem : HistoryItem
    {
        public TopicHandle Topic { get; set; }

        /// <summary>
        /// These are used to calculate major and minor weaknesses on the user profile.
        /// A failure on Understanding Answer Dimension means its a major weakness.
        /// Understanding Answer Dimension is Pass if Success / (Success + Failure) >= 0.8
        /// </summary>
        public Dictionary<AnswerDimension, int> AnswerDimensionSuccessCount { get; set; }
        public Dictionary<AnswerDimension, int> AnswerDimensionFailureCount { get; set; }

        public Dictionary<string, int> UmbrellaTopicSuccessCount { get; set; }
        public Dictionary<string, int> UmbrellaTopicFailureCount { get; set; }

        /// <summary>
        ///  This is only filled in when the topic moves to the user's topic history list.
        ///  When this item exists in the current state of the user, this is false.
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// We record the fact that the user skipped this topic. 
        /// FUTURE: We might use this drive certain decisions.
        /// </summary>
        public bool IsSkipped { get; set; }

        /// <summary>
        /// Gets or sets the is pseudo topic.
        /// </summary>
        /// <value>The is pseudo topic.</value>
        public bool IsPseudoTopic { get; set; }

        /// <summary>
        /// Gets or sets the weakness for pseudo topic.
        /// </summary>
        /// <value>The weakness for pseudo topic.</value>
        public Weakness WeaknessForPseudoTopic { get; set; }

        public TopicHistoryItem()
        {
            this.Topic = null;
            this.AnswerDimensionFailureCount = new Dictionary<AnswerDimension, int>();
            this.AnswerDimensionSuccessCount = new Dictionary<AnswerDimension, int>();
            this.UmbrellaTopicFailureCount = new Dictionary<string, int>();
            this.UmbrellaTopicSuccessCount = new Dictionary<string, int>();
            this.IsPseudoTopic = false;
            this.IsSkipped = false;
            this.IsSuccessful = false;
            this.WeaknessForPseudoTopic = null;
        }

        public override bool Equals(object obj)
        {
            return (obj as TopicHistoryItem).Topic.Equals(this.Topic);
        }

        public override int GetHashCode()
        {
            return this.Topic.GetHashCode();
        }
    }
}