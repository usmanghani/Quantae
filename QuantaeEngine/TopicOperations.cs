using Quantae.DataModel;
using System;
using System.Linq;

namespace Quantae.Engine
{
    public class TopicOperations
    {
        public static void UpdateAnswerDimensionCounts(UserProfile profile, AnswerDimension answerDimension, AnswerScore score)
        {
            if (score == AnswerScore.Right)
            {
                profile.CurrentState.CourseLocationInfo.CurrentTopic.AnswerDimensionSuccessCount[answerDimension]++;
            }
            else if (score == AnswerScore.Wrong)
            {
                profile.CurrentState.CourseLocationInfo.CurrentTopic.AnswerDimensionFailureCount[answerDimension]++;
            }
        }

        public static Topic GetTopicFromHandle(TopicHandle handle)
        {
            return Repositories.Repositories.Topics.GetItemByHandle(handle);
        }

        // Move this out. One of these things is not like the other.
        public static void MarkCurrentTopicComplete(UserProfile userProfile)
        {
            var currentTopicHistoryItem = userProfile.CurrentState.CourseLocationInfo.CurrentTopic;
            var currentTopicHandle = currentTopicHistoryItem.Topic;

            bool isSuccess = TopicPolicies.IsTopicSuccessful(currentTopicHistoryItem);

            // update each failed topic count. This is the count that will trigger a back path.
            foreach (var k in userProfile.History.FailureCounters.Keys)
            {
                userProfile.History.FailureCounters[k]++;
            }

            if (!isSuccess)
            {
                userProfile.History.FailureCounters.Add(currentTopicHandle, 0);
            }
            else
            {
                // BUG: Check if it exists in the failure counters.
                userProfile.History.FailureCounters.Remove(currentTopicHandle);
            }

            // NOTE: we dont need to update the final grammar score since it will be updated after each topic.
            // Update all counters here.
            // TODO: Pull these into a separate function.
            if (WeaknessPolicies.IsGenderWeak(userProfile))
            {
                userProfile.Weaknesses[new Weakness() { WeaknessType = WeaknessType.GenderAgreement }]++;
            }

            if (WeaknessPolicies.IsNumberWeak(userProfile))
            {
                userProfile.Weaknesses[new Weakness() { WeaknessType = WeaknessType.NumberAgreement }]++;
            }

            foreach (var umbrellaTopic in userProfile.CurrentState.CourseLocationInfo.CurrentTopic.UmbrellaTopicSuccessCount.Keys)
            {
                if (WeaknessPolicies.IsUmbrellaTopicWeak(userProfile, umbrellaTopic))
                {
                    userProfile.Weaknesses[new Weakness() { WeaknessType = WeaknessType.UmbrellaTopic, UmbrellaTopicName = umbrellaTopic }]++;
                }
            }

            // Make it behave like a stack.
            currentTopicHistoryItem.LastTimestamp = DateTime.UtcNow;
            currentTopicHistoryItem.IsSuccessful = isSuccess;
            userProfile.History.TopicHistory.Insert(0, currentTopicHistoryItem);
            userProfile.CurrentState.CourseLocationInfo.CurrentTopic = null;
        }
    }
}
