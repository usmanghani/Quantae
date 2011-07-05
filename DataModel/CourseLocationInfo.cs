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

        /// <summary>
        /// Gets or sets the course location.
        /// </summary>
        /// <value>
        /// The course location.
        /// </value>
        public CourseLocation CourseLocation { get; set; }

        /// <summary>
        /// Gets or sets the state of the topic state machine.
        /// </summary>
        /// <value>
        /// The state of the topic state machine.
        /// </value>
        public TopicLocationInfo TopicLocationInfo { get; set; }

        public CourseLocationInfo()
        {
            this.CurrentTopic = null;
            this.CourseLocation = CourseLocation.LessonHub;
            this.TopicLocationInfo = new TopicLocationInfo();
        }
    }
}