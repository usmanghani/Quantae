using System;
using System.Collections.Generic;
using System.Linq;
using Quantae.DataModel;
using System.IO;
using System.Text.RegularExpressions;
using MongoDB.Driver.Builders;
using Quantae.Repositories;

namespace Quantae.Engine
{
    public interface IGraphNavigator
    {
        TopicHandle GetNextTopic(UserProfile userProfile);
    }

    public class TopicGraphNavigator : IGraphNavigator
    {
        private RepositoryBase<Topic> topicsRepository = null;

        private TopicGraphNavigator()
        {

        }

        public TopicGraphNavigator(RepositoryBase<Topic> topicRepository)
        {
            this.topicsRepository = topicRepository;
        }

        // Send only relevant items here.
        public TopicHandle GetNextTopic(UserProfile userProfile)
        {
            // ALGO:
            // What we need: 
            // 1. Whole topic history.
            // 2. Graph
            // 3. Failure counters
            // 4. Weaknesses.
            // What we are doing:
            // 1. Check failure counters to make sure we don't go very far from a failed topic.
            // 1.a. Trigger a failed topic only if its index is less than the current topic.
            // 2. Check Pseudo Topic Triggers (Major weaknesses)
            // 3. Keep moving forward
            //    For each topic encountered: 
            //       If Dependencies not satisfied, then skip OR if Already successful then skip.
            //       Otherwise go to it.
            // 4. Only when we run out of topics at the end, we go over the failure counters and
            //    present them

            #region Check Failure Counters

            // BUG: Update failure counters before return for all failed topics,
            // return target topic from here. and at the end of the function,
            // update the failure counters.
            // First make sure no failed topics are due.
            foreach (var t in userProfile.History.FailureCounters.Keys)
            {
                if (userProfile.History.FailureCounters[t] > 2)
                {
                    // reset the counter because we are hitting this topic now.
                    userProfile.History.FailureCounters[t] = 0;
                    return t;
                }
            }

            #endregion

            #region Check Pseudo Topic Triggers (Major weaknesses)

            // TODO: Figure out Pseudo Topic trigger logic.

            #endregion

            #region Look at history to figure out next topic.

            // Read the last topic in user history that is not a pseudo topic.
            // The list is reversed.
            // BUG: FirstOrDefault is not supposed to return the absolute first, only the first
            // it sees. Change topic history to a stack.
            TopicHistoryItem thi = userProfile.History.TopicHistory.FirstOrDefault(h => !h.IsPseudoTopic);

            // First try to move to the topic immediately following this one.
            // BUG: thi could be NULL
            Topic currentTopic = Repositories.Repositories.Topics.GetItemByHandle(thi.Topic);

            Topic candidateTopic = Repositories.Repositories.Topics.GetTopicByIndex(currentTopic.Index + 1);

            Topic targetTopic = null;

            bool dependenciesDone = TopicPolicies.AreDependenciesSatisfied(userProfile, candidateTopic);

            if (dependenciesDone)
            {
                targetTopic = candidateTopic;
            }
            else
            {
                // When we come here, it means the next immediate topic is out.
                // Let's try the last successful topic and keep going until we are successful or we run out of topics in the history.
                foreach (var historyItem in userProfile.History.TopicHistory)
                {
                    // BUG: Check for pseudo topic.
                    if (historyItem.IsSuccessful)
                    {
                        TopicHandle th = SelectForwardLink(userProfile, Repositories.Repositories.Topics.GetItemByHandle(historyItem.Topic));
                        if (th != null)
                        {
                            targetTopic = Repositories.Repositories.Topics.GetItemByHandle(th);
                            break;
                        }
                    }
                }

                int costOfMovingBack = int.MaxValue;

                if (targetTopic != null)
                {
                    costOfMovingBack = Math.Abs(currentTopic.Index - targetTopic.Index);
                    //if (currentTopic.Index > targetTopic.Index)
                    //    costOfMovingBack = currentTopic.Index - targetTopic.Index;
                    //else
                    //    costOfMovingBack = targetTopic.Index - currentTopic.Index;
                }

                // try to move fwd by selecting all topics with zero dependencies  and whose index is
                // great than the current topic's index.
                var cursor = Repositories.Repositories.Topics.Collection.Find(Query.And(Query.Size("Dependencies", 0), Query.GT("Index", currentTopic.Index)));

                // BUG: Check to see if any of them has been successful.
                foreach (var topic in cursor)
                {
                    if (topic.Index - currentTopic.Index < costOfMovingBack)
                    {
                        targetTopic = topic;
                        break;
                    }
                }

                if (cursor.Count() == 0)
                {
                    bool found = false;
                    while (!found)
                    {
                        candidateTopic = Repositories.Repositories.Topics.GetTopicByIndex(candidateTopic.Index + 1);

                        if (candidateTopic == null)
                        {
                            break;
                        }

                        bool dependenciesSatisfied = TopicPolicies.AreDependenciesSatisfied(userProfile, candidateTopic);

                        if (!dependenciesSatisfied)
                        {
                            continue;
                        }

                        if (candidateTopic.Index - currentTopic.Index < costOfMovingBack)
                        {
                            targetTopic = candidateTopic;
                            found = true;
                        }
                    }
                }
            }

            #endregion

            # region move them out to utilities or something. Cz they dont belong here.
            
            if (targetTopic != null)
            {
                UpdateFailedTopicsCount(userProfile);
                UpdateCurrentTopicState(userProfile, new TopicHandle(targetTopic));
                return new TopicHandle(targetTopic);
            }

            #endregion 

            // TODO: Figure out what to do in the orchestrator when we are stuck.
            // FIX: Return a rich result type that tells you the reason.
            return null;
        }

        // Move this out. One of these things is not like the other.
        public void MarkTopicComplete(UserProfile userProfile, TopicHandle topicHandle)
        {
            bool isSuccess = TopicPolicies.IsTopicSuccessful(userProfile.CurrentState.CourseLocationInfo.CurrentTopic);

            // update each failed topic count. This is the count that will trigger a back path.
            foreach (var k in userProfile.History.FailureCounters.Keys)
            {
                userProfile.History.FailureCounters[k]++;
            }

            if (!isSuccess)
            {
                userProfile.History.FailureCounters.Add(topicHandle, 0);
            }
            else
            {
                // BUG: Check if it exists in the failure counters.
                userProfile.History.FailureCounters.Remove(topicHandle);
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
            userProfile.History.TopicHistory.Insert(0, userProfile.CurrentState.CourseLocationInfo.CurrentTopic);
            userProfile.History.TopicHistory.First().LastTimestamp = DateTime.UtcNow;
            userProfile.History.TopicHistory.First().IsSuccessful = isSuccess;

            userProfile.CurrentState.CourseLocationInfo.CurrentTopic = null;
        }

        private TopicHandle SelectForwardLink(UserProfile userProfile, Topic currentTopic)
        {
            // Next topic's dependencies were not satisfied.
            // Get forward links and try to find a candidate node.
            foreach (var candidateTopic in currentTopic.ForwardLinks)
            {
                var candidate = Repositories.Repositories.Topics.GetItemByHandle(candidateTopic);

                if (TopicPolicies.AreDependenciesSatisfied(userProfile, candidate) &&
                    !TopicPolicies.HasUserSuccessfullyDoneTopic(userProfile, candidateTopic))
                {
                    return candidateTopic;
                }
            }

            return null;
        }

        #region Take 'em ooout.

        private void UpdateCurrentTopicState(UserProfile userProfile, TopicHandle nextTopic)
        {
            userProfile.CurrentState.CourseLocationInfo.CurrentTopic = new TopicHistoryItem() { Topic = nextTopic };
        }

        private void UpdateFailedTopicsCount(UserProfile userProfile)
        {
            foreach (var t in userProfile.History.FailureCounters.Keys)
            {
                userProfile.History.FailureCounters[t]++;
            }
        }

        #endregion

    }
}
