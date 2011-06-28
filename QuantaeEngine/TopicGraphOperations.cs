﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;
using Quantae.Repositories;
using System.IO;
using System.Text.RegularExpressions;

namespace Quantae.Engine
{
    public class TopicGraphOperations
    {
        public static void CreateForwardLinks()
        {
            int startIndex = 1;
            int count = Repositories.Repositories.Topics.CountItems();
            foreach (var i in Enumerable.Range(1, count))
            {
                Topic t = Repositories.Repositories.Topics.GetTopicByIndex(i);

                if (t.Dependencies == null || t.Dependencies.Count == 0)
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

                    f.ForwardLinks.Add(new TopicHandle(t));

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
            return str.Equals("Past") || str.Equals("PresentFuture");
        }

        public static TopicHandle GetNextTopic(UserProfile userProfile)
        {
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

            // Read the last topic in user history.
            TopicHistoryItem thi = userProfile.TopicHistory.Last();

            Topic currentTopic = Repositories.Repositories.Topics.GetItemByHandle(thi.Topic);
            Topic nextTopic = Repositories.Repositories.Topics.GetTopicByIndex(currentTopic.Index + 1);

            if (TopicPolicies.IsTopicSuccessful(thi))
            {
                bool dependenciesDone = TopicPolicies.AreDependenciesSatisfied(userProfile, nextTopic);

                if (dependenciesDone)
                {
                    UpdateFailedTopicsCount(userProfile);
                    UpdateCurrentTopicState(userProfile, nextTopic);
                    return new TopicHandle(nextTopic);
                }

                // Next topic's dependencies were not satisfied.
                // Get forward links and try to find a candidate node.
                foreach (var candidateTopic in currentTopic.ForwardLinks)
                {
                    var candidate = Repositories.Repositories.Topics.GetItemByHandle(candidateTopic);

                    if (candidateTopic.Equals(new TopicHandle(nextTopic)))
                    {
                        continue;
                    }

                    dependenciesDone = TopicPolicies.AreDependenciesSatisfied(userProfile, candidate);

                    if (dependenciesDone)
                    {
                        return candidateTopic;
                    }
                }
            }

            return null;
        }

        public static void UpdateCurrentTopicState(UserProfile userProfile, Topic nextTopic)
        {
            userProfile.CurrentState.CourseStateMachineState.CurrentTopic = new TopicHistoryItem() { Topic = new TopicHandle(nextTopic) };
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
            bool isSuccess = TopicPolicies.IsTopicSuccessful(userProfile.CurrentState.CourseStateMachineState.CurrentTopic);

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
                userProfile.FailureCounters.Remove(topicHandle);
            }

            // NOTE: we dont need to update the final grammar score since it will be updated after each topic.

            if (WeaknessPolicies.IsGenderWeak(userProfile))
            {
                userProfile.Weaknesses[new Weakness() { WeaknessType = WeaknessType.GenderAgreement }]++;
            }

            if (WeaknessPolicies.IsNumberWeak(userProfile))
            {
                userProfile.Weaknesses[new Weakness() { WeaknessType = WeaknessType.NumberAgreement }]++;
            }

            foreach (var umbrellaTopic in userProfile.CurrentState.CourseStateMachineState.CurrentTopic.UmbrellaTopicSuccessCount.Keys)
            {
                if (WeaknessPolicies.IsUmbrellaTopicWeak(userProfile, umbrellaTopic))
                {
                    userProfile.Weaknesses[new Weakness() { WeaknessType = WeaknessType.UmbrellaTopic, UmbrellaTopicName = umbrellaTopic }]++;
                }
            }

            userProfile.TopicHistory.Add(userProfile.CurrentState.CourseStateMachineState.CurrentTopic);

            userProfile.CurrentState.CourseStateMachineState.CurrentTopic = null;
        }
    }
}
