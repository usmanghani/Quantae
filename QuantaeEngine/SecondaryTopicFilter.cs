using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class SecondaryTopicFilter : ISentenceFilter
    {
        public bool IsSentenceValid(UserProfile user, Sentence sentence)
        {
            var userTopicHistory = user.History.TopicHistory.Select(thi => thi.Topic);
            bool result = true;
            foreach (var topic in sentence.SecondaryTopics)
            {
                if (!userTopicHistory.Contains(topic))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}
