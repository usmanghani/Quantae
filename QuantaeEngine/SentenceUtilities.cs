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
        private interface IElementContext
        {
        }

        private interface IParserContext : IElementContext
        {
            IDictionary<int, IElementContext> VocabContexts { get; }
            IDictionary<int, IElementContext> GrammarContexts { get; }
            IDictionary<int, IElementContext> GrammarAnalysisContexts { get; }

            bool IsGrammarEntryTranslation { get; set; }

            int ValueIndex { get; set; }
            int LineNumber { get; set; }
            int ColIndex { get; set; }
        }

        private class ParserContext : IParserContext
        {
            public ParserContext()
            {
                this.VocabContexts = new Dictionary<int, IElementContext>();
                this.GrammarContexts = new Dictionary<int, IElementContext>();
                this.GrammarAnalysisContexts = new Dictionary<int, IElementContext>();
                this.QuestionContexts = new Dictionary<int, IElementContext>();
            }

            public IDictionary<int, IElementContext> VocabContexts { get; private set; }
            public IDictionary<int, IElementContext> GrammarContexts { get; private set; }
            public IDictionary<int, IElementContext> GrammarAnalysisContexts { get; private set; }
            public IDictionary<int, IElementContext> QuestionContexts { get; private set; }

            public bool IsGrammarEntryTranslation { get; set; }
            public int ValueIndex { get; set; }
            public int LineNumber { get; set; }
            public int ColIndex { get; set; }
        }

        private class VocabContext : IElementContext
        {
            public string Text { get; set; }
            public string Translation { get; set; }
            public string Conjugation { get; set; }
        }

        private class GrammarEntryContext : IElementContext
        {
            public string Text { get; set; }
            public string Translation { get; set; }
        }

        private class GrammarAnalysisContext : IElementContext
        {
            public string GrammarRole { get; set; }
            public string AnalysisEntry { get; set; }
        }

        private class QuestionContext : IElementContext
        {
            public string QuestionString { get; set; }
            public string QuestionSubstring { get; set; }
            public QuestionDimension Dimension { get; set; }
            public List<string> QuestionSelections { get; set; }

            public QuestionContext()
            {
                this.QuestionSelections = new List<string>();
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

            public static Func<ParserContext, string, Sentence, ParserContext> EmptyParseFunc = (ctx, val, s) => ctx;
            public static Dictionary<string, Func<ParserContext, string, Sentence, ParserContext>> ColumnProcessorMap =
                new Dictionary<string, Func<ParserContext, string, Sentence, ParserContext>>() 
            {
                { SentenceText, ProcessSentenceText }, 
                { SentenceTranslation, ProcessSentenceTranslation },  
                { VocabEntries, ProcessVocabEntry }, 
                { WordTranslation, ProcessWordTranslation }, 
                { Conjugation, ProcessConjugation },  
                { GrammarEntries, ProcessGrammarEntry },
                { GrammarRoles, ProcessGrammarRole },
                { Arrows, ProcessGrammaticalAnalysisElement },
                { Boxes, ProcessContextualAnalysisElement },
                { RoleConjugationPairs, ProcessRoleConjugationPair },
                { Question1, EmptyParseFunc },
                { Question2, EmptyParseFunc },
                { Question3, EmptyParseFunc },
                { Question4, EmptyParseFunc },
                { Question5, EmptyParseFunc },
                { QuestionString, ProcessQuestionString },
                { QuestionSubstring, ProcessQuestionSubstring },
                { QuestionDimensionColumn, ProcessQuestionDimension }, 
                { QuestionSelection01, ProcessQuestionSelection },
                { QuestionSelection02, ProcessQuestionSelection },
                { QuestionSelection03, ProcessQuestionSelection },
                { QuestionSelection04, ProcessQuestionSelection },
                { IncludedTopics, ProcessIncludedTopic },
                { Tags, ProcessTag }
            };

            public static bool IsColumnName(string token)
            {
                return ColumnNames.Contains(token, StringComparer.OrdinalIgnoreCase);
            }

            public static bool IsColumnValueComposite(string columnName)
            {
                return columnName.Equals(Arrows)
                    || columnName.Equals(Boxes)
                    || columnName.Equals(RoleConjugationPairs)
                    || columnName.Equals(Conjugation);
            }

            public static bool ShouldMaintainValueIndexCount(string columnName)
            {
                return columnName.Equals(VocabEntries)
                    || columnName.Equals(WordTranslation)
                    || columnName.Equals(Conjugation)
                    || columnName.Equals(GrammarEntries)
                    || columnName.Equals(GrammarRoles)
                    || columnName.Equals(Arrows);
            }

            public static ParserContext ProcessColumn(string colName, ParserContext context, string colValue, Sentence sentence)
            {
                if (!ColumnProcessorMap.ContainsKey(colName))
                {
                    throw new InvalidOperationException(string.Format("invalid column name {0}", colName));
                }

                return ColumnProcessorMap[colName](context, colValue, sentence);
            }

            public static ParserContext ProcessSentenceText(ParserContext context, string colValue, Sentence sentence)
            {
                sentence.SentenceText = colValue;
                return context;
            }

            public static ParserContext ProcessSentenceTranslation(ParserContext context, string colValue, Sentence sentence)
            {
                sentence.SentenceTranslation = colValue;
                return context;
            }

            public static ParserContext ProcessVocabEntry(ParserContext context, string colValue, Sentence sentence)
            {
                VocabEntryHandle handle = CreateVocabEntryIfNotExist(colValue);
                sentence.VocabEntries.Add(handle);

                VocabContext vc = new VocabContext() { Text = colValue };
                context.VocabContexts.Add(context.ValueIndex, vc);

                return context;
            }

            public static ParserContext ProcessWordTranslation(ParserContext context, string colValue, Sentence sentence)
            {
                if (!context.IsGrammarEntryTranslation)
                {
                    if (context.ValueIndex >= context.VocabContexts.Count())
                    {
                        // TODO: Throw a meaningful exception here.
                        ReportError("Value Index out of range when parsing vocab entry translation", context);
                    }

                    string vocabText = (context.VocabContexts[context.ValueIndex] as VocabContext).Text;
                    var results = Repositories.Repositories.Vocabulary.FindAs(
                        VocabQueries.GetVocabEntryByText(vocabText), indexHint: "Text");

                    if (results.Count() > 0)
                    {
                        results.First().Translation = colValue;
                        Repositories.Repositories.Vocabulary.Save(results.First());
                    }
                    else
                    {
                        // TODO: Vocab entry not found, throw meaningful error
                        ReportError(string.Format("Vocab entry not found {0}, translation {1}", vocabText, colValue), context);
                    }

                    (context.VocabContexts[context.ValueIndex] as VocabContext).Translation = colValue;
                }
                else
                {
                    if (context.ValueIndex >= context.GrammarContexts.Count())
                    {
                        // TODO: Throw a meaningful exception here.
                        ReportError("Value Index out of range when parsing grammar entry translation", context);
                    }

                    string grammarEntryText = (context.GrammarContexts[context.ValueIndex] as GrammarEntryContext).Text;
                    var results = Repositories.Repositories.GrammarEntries.FindAs(
                        GrammarEntryQueries.GetGrammarEntryByText(grammarEntryText), indexHint: "Text");

                    if (results.Count() > 0)
                    {
                        results.First().Translation = colValue;
                        Repositories.Repositories.GrammarEntries.Save(results.First());
                    }
                    else
                    {
                        // TODO: Vocab entry not found, throw meaningful error
                        ReportError(string.Format("Grammar entry not found {0}, translation {1}", grammarEntryText, colValue), context);
                    }

                    (context.GrammarContexts[context.ValueIndex] as GrammarEntryContext).Translation = colValue;

                }

                return context;
            }

            public static ParserContext ProcessConjugation(ParserContext context, string colValue, Sentence sentence)
            {
                if (context.ValueIndex >= context.VocabContexts.Count())
                {
                    // TODO: Throw a meaningful exception here.
                    ReportError("Value Index out of range when parsing vocab entry conjugation", context);
                }

                string vocabText = (context.VocabContexts[context.ValueIndex] as VocabContext).Text;
                var results = Repositories.Repositories.Vocabulary.FindAs(VocabQueries.GetVocabEntryByText(vocabText), indexHint: "Text");

                if (results.Count() > 0)
                {
                    results.First().Conjugation = ParseConjugation(colValue);
                    Repositories.Repositories.Vocabulary.Save(results.First());
                }
                else
                {
                    // TODO: Vocab entry not found, throw meaningful error
                    ReportError(string.Format("Vocab entry not found {0}, conjugation {1}", vocabText, colValue), context);
                }

                (context.VocabContexts[context.ValueIndex] as VocabContext).Conjugation = colValue;
                return context;
            }

            public static ParserContext ProcessGrammarEntry(ParserContext context, string colValue, Sentence sentence)
            {
                GrammarEntryHandle handle = CreateGrammarEntryIfNotExist(colValue);
                sentence.GrammarEntries.Add(handle);

                GrammarEntryContext gec = new GrammarEntryContext() { Text = colValue };
                context.GrammarContexts.Add(context.ValueIndex, gec);

                return context;
            }

            public static ParserContext ProcessGrammarRole(ParserContext context, string colValue, Sentence sentence)
            {
                GrammarRoleHandle handle = CreateGrammarRoleIfNotExist(colValue);

                GrammarAnalysisContext gac = new GrammarAnalysisContext() { GrammarRole = colValue };
                context.GrammarAnalysisContexts.Add(context.ValueIndex, gac);

                return context;
            }

            public static ParserContext ProcessGrammaticalAnalysisElement(ParserContext context, string colValue, Sentence sentence)
            {
                if (context.ValueIndex >= context.GrammarAnalysisContexts.Count())
                {
                    // TODO: Throw a meaningful exception here.
                    ReportError("Value Index out of range when parsing grammar role analysis entry", context);
                }

                string roleText = (context.GrammarAnalysisContexts[context.ValueIndex] as GrammarAnalysisContext).GrammarRole;
                var results = Repositories.Repositories.GrammarRoles.FindAs(GrammarRoleQueries.GetGrammarRoleByName(roleText), indexHint: "RoleName");

                if (results.Count() <= 0)
                {
                    // TODO: Grammar Role not found, throw meaningful error
                    ReportError(string.Format("Grammar Role not found {0}", roleText), context);
                }

                var tuple = ParseArrows(colValue);

                GrammarAnalysisElement gae = new GrammarAnalysisElement();
                GrammarRoleHandle startingRoleHandle = null;
                if (tuple.Item1.Count == 1 && tuple.Item1[0] != 0)
                {
                    GrammarAnalysisContext gac = (context.GrammarAnalysisContexts[tuple.Item1[0]] as GrammarAnalysisContext);
                    gac.AnalysisEntry = colValue;
                    string startingRoleText = gac.GrammarRole;
                    GrammarRole startingRole = Repositories.Repositories.GrammarRoles.FindOneAs(GrammarRoleQueries.GetGrammarRoleByName(startingRoleText));
                    if (startingRole == null)
                    {
                        ReportError(string.Format("Grammar Role not found {0}", startingRoleText), context);
                    }

                    startingRoleHandle = new GrammarRoleHandle(startingRole);
                }

                gae.StartSegmentRolePair = new QuantaeTuple<List<int>, GrammarRoleHandle>(tuple.Item1, startingRoleHandle);
                gae.EndSegmentRolePair = new QuantaeTuple<List<int>, GrammarRoleHandle>(tuple.Item2, new GrammarRoleHandle(results.First()));

                sentence.GrammarAnalysis.Add(gae);

                return context;
            }

            public static ParserContext ProcessContextualAnalysisElement(ParserContext context, string colValue, Sentence sentence)
            {
                string processedValue = colValue.Replace("\"", "").Trim();
                string[] indices = processedValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                List<int> list = new List<int>();
                foreach (var i in indices)
                {
                    int a = int.Parse(i.Trim());
                    list.Add(a);
                }

                sentence.ContextualAnalysis.Add(list.ToArray());

                return context;
            }

            public static ParserContext ProcessRoleConjugationPair(ParserContext context, string colValue, Sentence sentence)
            {
                string role = string.Empty;
                Conjugation conjugation = ParseRules(colValue, out role);
                role = role.Trim();
                if (!string.IsNullOrEmpty(role) && conjugation != null)
                {
                    CreateGrammarRoleIfNotExist(role);
                    GrammarRole gr = Repositories.Repositories.GrammarRoles.FindOneAs(GrammarRoleQueries.GetGrammarRoleByName(role));
                    QuantaeTuple<GrammarRoleHandle, DataModel.Conjugation> tuple =
                        new QuantaeTuple<GrammarRoleHandle, DataModel.Conjugation>(new GrammarRoleHandle(gr), conjugation);
                    sentence.RoleConjugationPairs.Add(tuple);
                }

                return context;
            }

            public static ParserContext ProcessQuestion(ParserContext context, string colValue, Sentence sentence)
            {
                // DO NOTHING FOR NOW.
                // Since there is not value associated with this column, this method will never be called.
                return context;
            }

            public static ParserContext ProcessQuestionString(ParserContext context, string colValue, Sentence sentence)
            {
                QuestionContext qc = new QuestionContext();
                qc.QuestionString = colValue;
                context.QuestionContexts.Add(context.QuestionContexts.Count, qc);
                return context;
            }

            public static ParserContext ProcessQuestionSubstring(ParserContext context, string colValue, Sentence sentence)
            {
                QuestionContext qc = (QuestionContext)context.QuestionContexts[context.QuestionContexts.Count - 1];
                qc.QuestionSubstring = colValue;
                return context;
            }

            public static ParserContext ProcessQuestionDimension(ParserContext context, string colValue, Sentence sentence)
            {
                QuestionContext qc = (QuestionContext)context.QuestionContexts[context.QuestionContexts.Count - 1];
                qc.Dimension = (QuestionDimension)Enum.Parse(typeof(QuestionDimension), colValue);
                // we now have enough information to insert this question into the sentence.

                Question q = new Question();
                q.QuestionString = qc.QuestionString;
                q.QuestionSubstring = qc.QuestionSubstring;
                q.Dimension = qc.Dimension;

                sentence.Questions.Add(qc.Dimension, q);

                return context;
            }

            public static ParserContext ProcessQuestionSelection(ParserContext context, string colValue, Sentence sentence)
            {
                QuestionContext qc = (QuestionContext)context.QuestionContexts[context.QuestionContexts.Count - 1];
                qc.QuestionSelections.Add(colValue);
                AnswerChoice choice = new AnswerChoice();
                choice.Answer = colValue;
                sentence.Questions[qc.Dimension].AnswerChoices.Add(choice);
                return context;
            }

            public static ParserContext ProcessIncludedTopic(ParserContext context, string colValue, Sentence sentence)
            {
                int topicIndex = int.Parse(colValue.Trim());
                Topic t = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(topicIndex));
                sentence.SecondaryTopics.Add(new TopicHandle(t));
                return context;
            }

            public static ParserContext ProcessTag(ParserContext context, string colValue, Sentence sentence)
            {
                sentence.Tags.Add(colValue.Trim());
                return context;
            }

            private static VocabEntryHandle CreateVocabEntryIfNotExist(string vocab)
            {
                VocabEntry entry = null;
                var results = Repositories.Repositories.Vocabulary.FindAs(
                    VocabQueries.GetVocabEntryByText(vocab), indexHint: "Text");

                if (results.Count() > 0)
                {
                    entry = results.First();
                }
                else
                {
                    entry = new VocabEntry();
                    entry.Text = vocab;
                    Repositories.Repositories.Vocabulary.Save(entry);
                }

                return new VocabEntryHandle(entry);
            }

            private static GrammarEntryHandle CreateGrammarEntryIfNotExist(string grammarEntry)
            {
                GrammarEntry entry = null;
                var results = Repositories.Repositories.GrammarEntries.FindAs(
                    GrammarEntryQueries.GetGrammarEntryByText(grammarEntry), indexHint: "Text");

                if (results.Count() > 0)
                {
                    entry = results.First();
                }
                else
                {
                    entry = new GrammarEntry();
                    entry.Text = grammarEntry;
                    Repositories.Repositories.GrammarEntries.Save(entry);
                }

                return new GrammarEntryHandle(entry);
            }

            private static GrammarRoleHandle CreateGrammarRoleIfNotExist(string role)
            {
                GrammarRole gr = null;
                var results = Repositories.Repositories.GrammarRoles.FindAs(
                    GrammarRoleQueries.GetGrammarRoleByName(role), indexHint: "RoleName");

                if (results.Count() > 0)
                {
                    gr = results.First();
                }
                else
                {
                    gr = new GrammarRole();
                    gr.RoleName = role;
                    Repositories.Repositories.GrammarRoles.Save(gr);
                }

                return new GrammarRoleHandle(gr);
            }

            private static Tuple<List<int>, List<int>> ParseArrows(string arrowsStr)
            {
                string[] tokens = arrowsStr.Split("<--".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] startIndicesStrings = tokens[1].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] endIndicesStrings = tokens[0].Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                List<int> startIndices = new List<int>();
                foreach (var i in startIndicesStrings)
                {
                    startIndices.Add(int.Parse(i));
                }

                List<int> endIndices = new List<int>();
                foreach (var i in endIndicesStrings)
                {
                    endIndices.Add(int.Parse(i));
                }

                return Tuple.Create(startIndices, endIndices);
            }

            private static Conjugation ParseConjugation(string colValue)
            {
                string processed = colValue.Replace("\"", "");
                processed = processed.Remove(processed.Length - 1);
                var conjs = processed.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                Conjugation conjugation = null;
                if (conjs.Length <= 2)
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
                else
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

                return conjugation;
            }

            private static void ReportError(string message, ParserContext context)
            {

                string msg = string.Format("{0} @ {1}, {2}", context.LineNumber, context.ColIndex);
                throw new InvalidDataException(msg);
            }

            private static Conjugation ParseRules(string rules, out string role)
            {
                role = string.Empty;
                if (string.IsNullOrWhiteSpace(rules))
                {
                    return null;
                }

                string regex = @"((?<role>\(.*?\))(?<conj>(,(.*?))*);)*";

                var matches = Regex.Matches(rules, regex);
                Match m = matches[0];
                role = m.Groups["role"].Value;
                role = role.Replace("(", "").Replace(")", "");

                Conjugation conjugation = null;
                string conj = m.Groups["conj"].Value;
                if (!string.IsNullOrEmpty(conj))
                {
                    conjugation = ParseConjugation(conj);
                }

                return conjugation;
            }

            private static bool IsGender(string str)
            {
                return str.Equals("Masculine") || str.Equals("Feminine") || str.Equals("Neutral");
            }

            private static bool IsNumber(string str)
            {
                return str.Equals("Singular") || str.Equals("Plural") || str.Equals("Dual");
            }

            private static bool IsPerson(string str)
            {
                return str.Equals("First") || str.Equals("Second") || str.Equals("Third");
            }

            private static bool IsTense(string str)
            {
                return str.Equals("Past") || str.Equals("PresentFuture") || str.Equals("Command");
            }

        }


        public static void PopulateSentences(string filename, int topic)
        {
            Repositories.Repositories.Vocabulary.EnsureIndex(new string[] { "Text" });
            Repositories.Repositories.GrammarEntries.EnsureIndex(new string[] { "Text" });

            ParserContext context = new ParserContext();
            context.LineNumber = 0;

            var lines = File.ReadAllLines(filename);
            foreach (var line in lines)
            {
                ++context.LineNumber;
                Sentence sentence = new Sentence();
                string currentColName = string.Empty;

                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var tokens = line.Split("\t".ToCharArray());
                context.ColIndex = 0;
                foreach (var token in tokens)
                {
                    ++context.ColIndex;
                    // 1. if the token is a column name.
                    if (Columns.IsColumnName(token.Trim()))
                    {
                        if (token.Trim().Equals(Columns.WordTranslation)
                            && currentColName.Equals(Columns.GrammarEntries))
                        {
                            context.IsGrammarEntryTranslation = true;
                        }

                        currentColName = token.Trim();
                        context.ValueIndex = -1;
                        continue;
                    }

                    // 2. else its a column value.
                    if (Columns.ShouldMaintainValueIndexCount(currentColName))
                    {
                        context.ValueIndex++;
                    }

                    context = Columns.ProcessColumn(currentColName, context, token.Trim(), sentence);
                }
            }
        }
    }
}
