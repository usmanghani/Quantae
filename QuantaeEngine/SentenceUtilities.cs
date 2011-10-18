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
    public class SentenceUtilities
    {
        public static void CreateForwardLinks()
        {
            var count = Repositories.Repositories.Topics.CountItems();
            for (var i = 0; i < count; i++)
            {
                Topic t = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(i));

                if (t == null || t.Dependencies == null || t.Dependencies.Count == 0)
                {
                    continue;
                }

                foreach (var d in t.Dependencies)
                {
                    Topic f = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(d));
                    if (f.ForwardLinks == null)
                    {
                        f.ForwardLinks = new List<int>();
                    }

                    if (!f.ForwardLinks.Contains(t.Index))
                    {
                        f.ForwardLinks.Add(t.Index);
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

                t.Dependencies = dependencies;

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

        private class Columns
        {
            public const string SentenceText = "Sentence";
            public const string SentenceTranslation = "Translation";
            public const string VocabEntries = "VocabEntries";
            public const string WordTranslation = "WordTranslation";
            public const string Conjugation = "Conjugation";
            public const string GrammarEntries = "GrammarEntries";
            public const string GrammarRoles = "GrammarRoles";
            public const string Arrows = "Arrows";
            public const string Boxes = "Boxes";
            public const string RoleConjugationPairs = "RoleConjugationPairs";
            public const string Question1 = "Question1";
            public const string Question2 = "Question2";
            public const string Question3 = "Question3";
            public const string Question4 = "Question4";
            public const string Question5 = "Question5";
            public const string QuestionString = "QuestionString";
            public const string QuestionSubstring = "QuestionSubstring";
            public const string QuestionDimensionColumn = "QuestionDimension";
            public const string QuestionSelection01 = "QuestionSelection01";
            public const string QuestionSelection02 = "QuestionSelection02";
            public const string QuestionSelection03 = "QuestionSelection03";
            public const string QuestionSelection04 = "QuestionSelection04";
            public const string IncludedTopics = "IncludedTopics";
            public const string Tags = "Tags";

            public static string[] ColumnNames = 
            {
                SentenceText, 
                SentenceTranslation, 
                VocabEntries, 
                WordTranslation, 
                Conjugation, 
                GrammarEntries, 
                GrammarRoles,
                Arrows,
                Boxes,
                RoleConjugationPairs,
                Question1,
                Question2,
                Question3,
                Question4,
                Question5,
                QuestionString,
                QuestionSubstring,
                QuestionDimensionColumn,
                QuestionSelection01,
                QuestionSelection02,
                QuestionSelection03,
                QuestionSelection04,
                IncludedTopics,
                Tags
            };

            public static Dictionary<string, Func<string, string, Sentence, string>> ColumnProcessorMap = new Dictionary<string, Func<string, string, Sentence, string>>() 
            {
                { SentenceText, ProcessSentenceText }, 
                { SentenceTranslation, ProcessSentenceTranslation },  
                { VocabEntries, ProcessVocabEntry }, 
                { WordTranslation, ProcessWordTranslation }, 
                { Conjugation, ProcessConjugation },  
                {GrammarEntries, ProcessGrammarEntry},
                { GrammarRoles, ProcessGrammarRole},
                {Arrows, ProcessGrammaticalAnalysisElement},
                {Boxes, ProcessContextualAnalysisElement},
                {RoleConjugationPairs, ProcessRoleConjugationPair},
                {Question1, ProcessQuestion},
                {Question2, ProcessQuestion},
                {Question3, ProcessQuestion},
                {Question4,ProcessQuestion},
                {Question5,ProcessQuestion},
                {QuestionString,ProcessQuestionString},
                {QuestionSubstring,ProcessQuestionSubstring},
                {QuestionDimensionColumn, ProcessQuestionDimension}, 
                {QuestionSelection01, ProcessQuestionSelection},
                {QuestionSelection02, ProcessQuestionSelection},
                {QuestionSelection03, ProcessQuestionSelection},
                {QuestionSelection04,ProcessQuestionSelection},
                {IncludedTopics, ProcessIncludedTopic},
                {Tags, ProcessTag}
            };

            public static bool IsColumn(string token)
            {
                return ColumnNames.Contains(token, StringComparer.OrdinalIgnoreCase);
            }

            public static string ProcessColumn(string colName, string context, string colValue, Sentence sentence)
            {
                if (!ColumnProcessorMap.ContainsKey(colName))
                {
                    throw new InvalidOperationException(string.Format("invalid column name {0}", colName));
                }

                return ColumnProcessorMap[colName](context, colValue, sentence);
            }

            public static string ProcessSentenceText(string context, string colValue, Sentence sentence)
            {
                sentence.SentenceText = colValue;
                return context;
            }

            public static string ProcessSentenceTranslation(string context, string colValue, Sentence sentence)
            {
                sentence.SentenceTranslation = colValue;
                return context;
            }

            public static string ProcessVocabEntry(string context, string colValue, Sentence sentence)
            {
                VocabEntryHandle handle = CreateVocabEntryIfNotExist(colValue);
                sentence.VocabEntries.Add(handle);
                return context;
            }

            public static string ProcessWordTranslation(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessConjugation(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessGrammarEntry(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessGrammarRole(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessGrammaticalAnalysisElement(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessContextualAnalysisElement(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessRoleConjugationPair(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessQuestion(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessQuestionString(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessQuestionSubstring(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessQuestionDimension(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessQuestionSelection(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessIncludedTopic(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            public static string ProcessTag(string context, string colValue, Sentence sentence)
            {
                return context;
            }

            private static VocabEntryHandle CreateVocabEntryIfNotExist(string colValue)
            {
                VocabEntry entry = null;
                var results = Repositories.Repositories.Vocabulary.FindAs(VocabQueries.GetVocabEntryByText(colValue), indexHint: "Text");
                if (results.Count() > 0)
                {
                    entry = results.First();
                }
                else
                {
                    entry = new VocabEntry();
                    entry.Text = colValue;
                }

                return new VocabEntryHandle(entry);
            }
        }

        public static void PopulateSentences(string filename, int topic)
        {
            Repositories.Repositories.Vocabulary.EnsureIndex(new string[] { "Text" });
            var lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                Sentence sentence = new Sentence();
                string context = string.Empty;
                string currentColName = string.Empty;

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var tokens = line.Split("\t".ToCharArray());

                foreach (var token in tokens)
                {
                    // 1. The token is either a col name.
                    if (Columns.IsColumn(token))
                    {
                        currentColName = token.Trim();
                        continue;
                    }

                    // 2. If its a column value.
                    context = Columns.ProcessColumn(currentColName, context, token, sentence);
                }
            }
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
