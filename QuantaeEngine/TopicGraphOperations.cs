using System;
using System.Collections.Generic;
using System.Linq;
using Quantae.DataModel;
using System.IO;
using System.Text.RegularExpressions;
using MongoDB.Driver.Builders;

namespace Quantae.Engine
{
    public class TopicGraphOperations
    {
        public static void CreateForwardLinks()
        {
            int count = Repositories.Repositories.Topics.CountItems();
            foreach (var i in Enumerable.Range(1, count))
            {
                Topic t = Repositories.Repositories.Topics.GetTopicByIndex(i);

                if (t == null || t.Dependencies == null || t.Dependencies.Count == 0)
                {
                    continue;
                }

                foreach (var d in t.Dependencies)
                {
                    Topic f = Repositories.Repositories.Topics.GetItemByHandle(d);
                    if (f.ForwardLinks == null)
                    {
                        f.ForwardLinks = new List<TopicHandle>();
                    }

                    var h = new TopicHandle(t);
                    if (!f.ForwardLinks.Contains(h))
                    {
                        f.ForwardLinks.Add(h);
                    }

                    Repositories.Repositories.Topics.Save(f);
                }
            }
        }

        public static void PopulateTopics(string filename)
        {
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var tokens = line.Split("\t".ToCharArray());

                int index = int.Parse(tokens[0]);
                string name = tokens[1];
                string rules = tokens[2];

                List<int> dependencies = new List<int>();
                if (tokens.Length > 3)
                {
                    dependencies.AddRange(tokens.Skip(3).Select(i => string.IsNullOrWhiteSpace(i) ? 0 : int.Parse(i)));
                }

                Topic t = new Topic() { Index = index, TopicName = name };

                t.RoleConjugationPairs = ParseRules(rules);

                t.Dependencies = ResolveDependencies(dependencies);

                if (!Repositories.Repositories.Topics.TopicExists(name) && !Repositories.Repositories.Topics.TopicExists(index))
                {
                    Repositories.Repositories.Topics.Save(t);
                }
                else // update dependency handles
                {
                    Topic t2 = Repositories.Repositories.Topics.GetTopicByIndex(index);
                    t2.Dependencies = t.Dependencies;
                    Repositories.Repositories.Topics.Save(t2);
                }
            }
        }

        public static List<TopicHandle> ResolveDependencies(List<int> dependencies)
        {
            List<TopicHandle> topicHandles = new List<TopicHandle>();
            foreach (var d in dependencies)
            {
                if (d == 0) continue;
                Topic t = Repositories.Repositories.Topics.GetTopicByIndex(d);
                if (t == null) continue;
                topicHandles.Add(new TopicHandle(t));
            }

            return topicHandles;
        }

        public static List<QuantaeTuple<GrammarRoleHandle, Conjugation>> ParseRules(string rules)
        {
            if (string.IsNullOrWhiteSpace(rules))
            {
                return Enumerable.Empty<QuantaeTuple<GrammarRoleHandle, Conjugation>>().ToList();
            }

            List<QuantaeTuple<GrammarRoleHandle, Conjugation>> list = new List<QuantaeTuple<GrammarRoleHandle, Conjugation>>();

            string regex = @"((?<role>\(.*?\))(?<conj>(,(.*?))*);)*";

            var matches = Regex.Matches(rules, regex);
            foreach (Match m in matches)
            {
                string role = m.Groups["role"].Value;

                if (string.IsNullOrWhiteSpace(role))
                {
                    continue;
                }

                role = role.Replace("(", "").Replace(")", "");

                GrammarRole gr = new GrammarRole() { RoleName = role };

                if (!Repositories.Repositories.GrammarRoles.GrammarRoleExists(gr.RoleName))
                {
                    Repositories.Repositories.GrammarRoles.Save(gr);
                }
                else
                {
                    gr = Repositories.Repositories.GrammarRoles.GetGrammarRolesByName(role).First();
                }

                GrammarRoleHandle grh = new GrammarRoleHandle(gr);

                string conj = m.Groups["conj"].Value;

                var conjs = conj.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                Conjugation conjugation = null;
                if (conjs.Length <= 2 && !role.Equals("Verb"))
                {
                    conjugation = new NounConjugation();

                    foreach (var c in conjs)
                    {
                        var c1 = c.Trim();
                        if (IsGender(c1))
                        {
                            conjugation.Gender = (GenderRule)Enum.Parse(typeof(GenderRule), c1, true);
                        }

                        if (IsNumber(c1))
                        {
                            conjugation.Number = (NumberRule)Enum.Parse(typeof(NumberRule), c1, true);
                        }
                    }
                }
                else if (conjs.Length > 2 || role.Equals("Verb"))
                {
                    conjugation = new VerbConjugation();

                    foreach (var c in conjs)
                    {
                        var c1 = c.Trim();

                        if (IsGender(c1))
                        {
                            conjugation.Gender = (GenderRule)Enum.Parse(typeof(GenderRule), c1, true);
                            continue;
                        }

                        if (IsNumber(c1))
                        {
                            conjugation.Number = (NumberRule)Enum.Parse(typeof(NumberRule), c1, true);
                            continue;
                        }

                        if (IsTense(c1))
                        {
                            (conjugation as VerbConjugation).Tense = (TenseRule)Enum.Parse(typeof(TenseRule), c1, true);
                            continue;
                        }

                        if (IsPerson(c1))
                        {
                            (conjugation as VerbConjugation).Person = (PersonRule)Enum.Parse(typeof(PersonRule), c1, true);
                            continue;
                        }
                    }
                }

                QuantaeTuple<GrammarRoleHandle, Conjugation> kvp = new QuantaeTuple<GrammarRoleHandle, Conjugation>(grh, conjugation);

                list.Add(kvp);
            }

            return list;
        }

        public static bool IsGender(string str)
        {
            return str.Equals("Masculine") || str.Equals("Feminine") || str.Equals("Neutral");
        }

        public static bool IsNumber(string str)
        {
            return str.Equals("Singular") || str.Equals("Plural") || str.Equals("Dual");
        }

        public static bool IsPerson(string str)
        {
            return str.Equals("First") || str.Equals("Second") || str.Equals("Third");
        }

        public static bool IsTense(string str)
        {
            return str.Equals("Past") || str.Equals("PresentFuture") || str.Equals("Command");
        }

        public static TopicHandle GetNextTopic(UserProfile userProfile)
        {
            // TODO: Figure out Pseudo Topic trigger logic.
            // BUG: Update failure counters before return for all failed topics,
            // return target topic from here. and at the end of the function,
            // update the failure counters.
            // First make sure no failed topics are due.
            foreach (var t in userProfile.FailureCounters.Keys)
            {
                if (userProfile.FailureCounters[t] > 2)
                {
                    // reset the counter because we are hitting this topic now.
                    userProfile.FailureCounters[t] = 0;
                    return t;
                }
            }

            // Read the last topic in user history that is not a pseudo topic.
            // The list is reversed.
            // BUG: FirstOrDefault is not supposed to return the absolute first, only the first
            // it sees. Change topic history to a stack.
            TopicHistoryItem thi = userProfile.TopicHistory.FirstOrDefault(h => !h.IsPseudoTopic);

            // First try to move to the topic immediately following this one.
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
                foreach (var historyItem in userProfile.TopicHistory)
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

            if (targetTopic != null)
            {
                UpdateFailedTopicsCount(userProfile);
                UpdateCurrentTopicState(userProfile, new TopicHandle(targetTopic));
                return new TopicHandle(targetTopic);
            }

            // TODO: Figure out what to do in the orchestrator when we are stuck.
            // FIX: Return a rich result type that tells you the reason.
            return null;
        }

        private static TopicHandle SelectForwardLink(UserProfile userProfile, Topic currentTopic)
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

        public static void UpdateCurrentTopicState(UserProfile userProfile, TopicHandle nextTopic)
        {
            userProfile.CurrentState.CourseLocationInfo.CurrentTopic = new TopicHistoryItem() { Topic = nextTopic };
        }

        public static void UpdateFailedTopicsCount(UserProfile userProfile)
        {
            foreach (var t in userProfile.FailureCounters.Keys)
            {
                userProfile.FailureCounters[t]++;
            }
        }

        public static void MarkTopicComplete(UserProfile userProfile, TopicHandle topicHandle)
        {
            bool isSuccess = TopicPolicies.IsTopicSuccessful(userProfile.CurrentState.CourseLocationInfo.CurrentTopic);

            // update each failed topic count. This is the count that will trigger a back path.
            foreach (var k in userProfile.FailureCounters.Keys)
            {
                userProfile.FailureCounters[k]++;
            }

            if (!isSuccess)
            {
                userProfile.FailureCounters.Add(topicHandle, 0);
            }
            else
            {
                // BUG: Check if it exists in the failure counters.
                userProfile.FailureCounters.Remove(topicHandle);
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
            userProfile.TopicHistory.Insert(0, userProfile.CurrentState.CourseLocationInfo.CurrentTopic);
            userProfile.TopicHistory.First().LastTimestamp = DateTime.UtcNow;
            userProfile.TopicHistory.First().IsSuccessful = isSuccess;

            userProfile.CurrentState.CourseLocationInfo.CurrentTopic = null;
        }
    }
}
