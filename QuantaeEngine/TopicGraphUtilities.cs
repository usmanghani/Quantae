using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;
using System.IO;
using System.Text.RegularExpressions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Quantae.Repositories;

namespace Quantae.Engine
{
    public class TopicGraphUtilities
    {
        public static void CreateForwardLinks()
        {
            int count = Repositories.Repositories.Topics.CountItems();
            foreach (var i in Enumerable.Range(1, count))
            {
                Topic t = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(i));

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

                t.GrammarRoles = ParseRules(rules);

                t.Dependencies = ResolveDependencies(dependencies);

                bool topicExistsByName = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByName(name)) != null;
                bool topicExistsByIndex = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(index)) != null;
                if (!topicExistsByName && !topicExistsByIndex)
                {
                    Repositories.Repositories.Topics.Save(t);
                }
                else // update dependency handles
                {
                    Topic t2 = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(index));
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
                Topic t = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(d));
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

                if (Repositories.Repositories.GrammarRoles.FindOneAs(GrammarRoleQueries.GetGrammarRoleByName(gr.RoleName)) == null)
                {
                    Repositories.Repositories.GrammarRoles.Save(gr);
                }
                else
                {
                    gr = Repositories.Repositories.GrammarRoles.FindAs(GrammarRoleQueries.GetGrammarRoleByName(role)).First();
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

    }
}
