using System;
using System.Collections.Generic;
using System.Linq;
using Quantae.DataModel;
using System.IO;
using System.Text.RegularExpressions;
using MongoDB.Driver.Builders;
using Quantae.Repositories;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Quantae.Engine
{
    public interface ITopicGraphNavigator
    {
        GetNextTopicResult GetNextTopic(UserProfile userProfile);
    }

    public class TopicGraphNavigator : ITopicGraphNavigator
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
        public GetNextTopicResult GetNextTopic(UserProfile userProfile)
        {
            // ALGO: GetNextTopic
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
            //       If Dependencies satisfied, and Not already successful then return topic.
            //       Otherwise go to next index.
            // 4. Only when we run out of topics at the end, we go over the failure counters and
            //    present them

            // Read the last topic in user history that is not a pseudo topic.
            // The list is reversed.
            // BUG: FirstOrDefault is not supposed to return the absolute first, only the first
            // it sees. Change topic history to a stack.
            TopicHistoryItem thi = userProfile.History.TopicHistory.FirstOrDefault(h => !h.IsPseudoTopic);

            // First try to move to the topic immediately following this one.
            // BUG: thi could be NULL
            Topic currentTopic = Repositories.Repositories.Topics.GetItemByHandle(thi.Topic);

            Topic targetTopic = null;

            bool found = false;
            bool isPseudo = true;
            WeaknessType pseudoTopicType = WeaknessType.Unknown;

            #region Check Failure Counters

            // BUG: Update failure counters before return for all failed topics,
            // return target topic from here. and at the end of the function,
            // update the failure counters.
            // First make sure no failed topics are due.
            foreach (var t in userProfile.History.FailureCounters.Keys)
            {
                Topic failedTopic = this.topicsRepository.GetItemByHandle(t);

                if ((failedTopic.Index > currentTopic.Index) &&
                    (userProfile.History.FailureCounters[t] > 2))
                {
                    // reset the counter because we are hitting this topic now.
                    userProfile.History.FailureCounters[t] = 0;
                    targetTopic = failedTopic;
                    found = true;
                }
            }

            #endregion

            #region Check Pseudo Topic Triggers (Major weaknesses)

            // TODO: Figure out Pseudo Topic trigger logic.

            if (!found)
            {
                var majorWeaknesses = userProfile.Weaknesses.Where(kvp => WeaknessPolicies.IsMajorWeakness(kvp.Key, kvp.Value));
                pseudoTopicType = majorWeaknesses.ElementAt(0).Key.WeaknessType;
                found = true;
                isPseudo = true;
            }

            #endregion

            #region Look at history to figure out next topic.

            if (!found)
            {
                int currentIndex = currentTopic.Index + 1;
                Topic candidateTopic = this.topicsRepository.FindOneAs(TopicQueries.GetTopicByIndex(currentIndex));

                while (!found)
                {
                    bool dependenciesDone = TopicPolicies.AreDependenciesSatisfied(userProfile, candidateTopic);
                    bool isSuccessful = TopicPolicies.HasUserSuccessfullyDoneTopic(userProfile, new TopicHandle(candidateTopic));
                    if (dependenciesDone && !isSuccessful)
                    {
                        targetTopic = candidateTopic;
                        found = true;
                    }
                    else
                    {
                        currentIndex++;
                        candidateTopic = this.topicsRepository.FindOneAs(TopicQueries.GetTopicByIndex(currentIndex));
                    }
                }
            }

            #endregion

            if (!found)
            {
                if (userProfile.History.FailureCounters.Count > 0)
                {
                    targetTopic = this.topicsRepository.GetItemByHandle(userProfile.History.FailureCounters.First().Key);
                    found = true;
                }
            }

            return new GetNextTopicResult(found, isPseudo, pseudoTopicType, targetTopic);


            // TODO: Move this to topic utilities when done.
            //# region move them out to utilities or something. Cz they dont belong here.

            //if (targetTopic != null)
            //{
            //    UpdateFailedTopicsCount(userProfile);
            //    UpdateCurrentTopicState(userProfile, new TopicHandle(targetTopic));
            //    return targetTopic;
            //}

            //#endregion

        }
    }
}
