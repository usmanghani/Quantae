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
            Topic currentTopic = Repositories.Repositories.Topics.GetItemByHandle(user.CurrentState.CourseLocationInfo.CurrentTopic.Topic);
            if (!currentTopic.IsPseudoTopic)
            {
                return user.CurrentState.CourseLocationInfo.CurrentTopic.Topic.Equals(sentence.PrimaryTopic);
            }

            return true;
        }
    }
}
