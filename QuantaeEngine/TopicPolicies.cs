using System.Linq;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class TopicPolicies
    {
        private static double topicSuccessPercentage = 0.8;
        private static int minQuestions = 20;
        private static int maxQuestions = 30;
        private static double sampleSentencePassPercentage = 0.7;

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
                var topicHistoryEntry = userProfile.History.TopicHistory.Where(thi => thi.Topic.Equals(th)).FirstOrDefault();
                if (topicHistoryEntry != null && topicHistoryEntry.IsSuccessful)
                {
                    return true;
                }

                return false;
            });
        }

        public static bool HasUserSuccessfullyDoneTopic(UserProfile userProfile, TopicHandle topic)
        {
            // TODO: Figure out if IsSuccessful condition is required or not. This might potentially lead to infinite
            // loops since we will keep forcing the user to cover failed topics. Those are already taken care of by
            // failure counters.
            return userProfile.History.TopicHistory.Any(thi => thi.Topic.Equals(topic) /*&& thi.IsSuccessful*/);
        }

        /// <summary>
        /// Determines whether if the sample section is complete for the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns></returns>
        public static bool IsSampleSectionComplete(UserProfile profile)
        {
            if (profile.CurrentState.TopicLocationInfo.QuestionCount < minQuestions)
            {
                return false;
            }

            if (profile.CurrentState.TopicLocationInfo.QuestionCount >= maxQuestions)
            {
                return true;
            }

            if (GetUnderstandingScore(profile) >= sampleSentencePassPercentage)
            {
                return true;
            }

            return false;
        }

        public static double GetUnderstandingScore(UserProfile profile)
        {
            int successCount = profile.CurrentState.CourseLocationInfo.CurrentTopic.AnswerDimensionSuccessCount[AnswerDimension.Understanding];
            int failureCount = profile.CurrentState.CourseLocationInfo.CurrentTopic.AnswerDimensionFailureCount[AnswerDimension.Understanding];

            return (double)successCount / (successCount + failureCount);
        }
    }
}
