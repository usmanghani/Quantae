using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class PrimaryTopicFilter : ISentenceFilter
    {
        public bool IsSentenceValid(UserProfile user, Sentence sentence)
        {
            TopicHistoryItem currentTopic = user.CurrentState.CourseLocationInfo.CurrentTopic;
            if (!currentTopic.IsPseudoTopic)
            {
                return currentTopic.Topic.Equals(sentence.PrimaryTopic);
            }

            return true;
        }
    }
}
