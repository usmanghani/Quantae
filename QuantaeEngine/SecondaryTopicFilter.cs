﻿using System;
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
            var userTopicHistory = user.TopicHistory.Select(thi => thi.Topic);

            foreach (var topic in sentence.SecondaryTopics)
            {
                if (!userTopicHistory.Contains(topic))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
