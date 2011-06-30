using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class CourseStateMachineState
    {
        /// <summary>
        /// Gets or sets the current topic.
        /// </summary>
        /// <value>
        /// The current topic.
        /// </value>
        public TopicHistoryItem CurrentTopic { get; set; }

        public CourseStateMachineState ()
        {
            this.CurrentTopic = null;
        }
    }
}
