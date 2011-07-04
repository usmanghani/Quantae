namespace Quantae.DataModel.Sql
{
    public class CourseLocationInfo : QuantaeObject
    {
        /// <summary>
        /// Gets or sets the current topic.
        /// </summary>
        /// <value>
        /// The current topic.
        /// </value>
        public TopicHistoryItem CurrentTopic { get; set; }

        public CourseLocationInfo()
        {
            this.CurrentTopic = null;
        }
    }
}