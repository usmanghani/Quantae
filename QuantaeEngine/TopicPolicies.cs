using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class TopicPolicies
    {
        private static double topicSuccessPercentage = 0.8;

        public static bool IsTopicSuccessful(TopicHistoryItem thi)
        {
            var understandingFailures = thi.AnswerDimensionFailureCount.Where(a => a.Key == AnswerDimension.Understanding).Sum(kvp => kvp.Value);
            var understandingSuccesses = thi.AnswerDimensionSuccessCount.Where(a => a.Key == AnswerDimension.Understanding).Sum(kvp => kvp.Value);

            var understandingScore = (double)understandingSuccesses / (understandingSuccesses + understandingFailures);
            return (understandingScore >= topicSuccessPercentage);
        }

        public static bool AreDependenciesSatisfied(UserProfile userProfile, Topic nextTopic)
        {
            return nextTopic.Dependencies.All(th =>
            {
                var topicHistoryEntry = userProfile.TopicHistory.Where(thi => thi.Topic.Equals(th)).FirstOrDefault();
                if (topicHistoryEntry != null && topicHistoryEntry.IsSuccessful)
                {
                    return true;
                }

                return false;
            });
        }
    }
}
