using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class PrimaryTopicFilter
    {
        public bool IsSentenceValid(UserProfile user, Sentence sentence)
        {
            Topic currentTopic = Repositories.Repositories.Topics.GetItemByHandle(user.CurrentState.CourseStateMachineState.CurrentTopic.Topic);
            if (!currentTopic.IsPseudoTopic)
            {
                return user.CurrentState.CourseStateMachineState.CurrentTopic.Topic.Equals(sentence.PrimaryTopic);
            }

            return true;
        }
    }
}
