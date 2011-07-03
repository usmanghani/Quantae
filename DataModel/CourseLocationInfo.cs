namespace Quantae.DataModel
{
    public class CourseLocationInfo
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