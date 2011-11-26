using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;
using System.IO;
using System.Text.RegularExpressions;

namespace Quantae.ParserLibrary
{
    public class ParsingException : Exception
    {
        internal ParsingException(IParserContext context, Exception innerException)
            : base(ComposeMessage(context), innerException)
        {
        }

        private static string ComposeMessage(IParserContext context)
        {
            return string.Format(
                "{0},{1}@{2}: Col: {3}, Value: {4}", 
                context.LineNumber, 
                context.ColIndex, 
                context.FileName, 
                context.CurrentColName, 
                context.CurrentColValue);
        }
    }

    internal interface IElementContext
    {
    }

    internal interface IParserContext : IElementContext
    {
        IParserRepository RepositoryContext { get; }

        IDictionary<int, IElementContext> VocabContexts { get; }
        IDictionary<int, IElementContext> GrammarContexts { get; }
        IDictionary<int, IElementContext> GrammarAnalysisContexts { get; }
        IDictionary<int, IElementContext> QuestionContexts { get; }

        string FileName { get; set; }
        int LineNumber { get; set; }
        int ColIndex { get; set; }
        bool IsGrammarEntryTranslation { get; set; }
        int ValueIndex { get; set; }
        string CurrentColName { get; set; }
        int PrimaryTopic { get; set; }
        string CurrentColValue { get; set; }
        Sentence Sentence { get; set; }
        bool IsExistingSentence { get; set; }
        bool HasSentenceBeenCleared { get; set; }
    }

    internal class ParserContext : IParserContext
    {
        public ParserContext(IParserRepository repositoryContext)
        {
            this.RepositoryContext = repositoryContext;

            this.VocabContexts = new Dictionary<int, IElementContext>();
            this.GrammarContexts = new Dictionary<int, IElementContext>();
            this.GrammarAnalysisContexts = new Dictionary<int, IElementContext>();
            this.QuestionContexts = new Dictionary<int, IElementContext>();
        }

        public IParserRepository RepositoryContext { get; private set; }

        public IDictionary<int, IElementContext> VocabContexts { get; private set; }
        public IDictionary<int, IElementContext> GrammarContexts { get; private set; }
        public IDictionary<int, IElementContext> GrammarAnalysisContexts { get; private set; }
        public IDictionary<int, IElementContext> QuestionContexts { get; private set; }

        public string FileName { get; set; }
        public int LineNumber { get; set; }
        public int ColIndex { get; set; }
        public bool IsGrammarEntryTranslation { get; set; }
        public int ValueIndex { get; set; }
        public int PrimaryTopic { get; set; }
        public string CurrentColValue { get; set; }
        public Sentence Sentence { get; set; }
        public string CurrentColName { get; set; }
        public bool IsExistingSentence { get; set; }
        public bool HasSentenceBeenCleared { get; set; }
    }

    internal class VocabContext : IElementContext
    {
        public string Text { get; set; }
        public string Translation { get; set; }
        public string Conjugation { get; set; }
    }

    internal class GrammarEntryContext : IElementContext
    {
        public string Text { get; set; }
        public string Translation { get; set; }
    }

    internal class GrammarAnalysisContext : IElementContext
    {
        public string GrammarRole { get; set; }
        public string AnalysisEntry { get; set; }
    }

    internal class QuestionContext : IElementContext
    {
        public string QuestionString { get; set; }
        public string QuestionSubstring { get; set; }
        public QuestionDimension Dimension { get; set; }
        public Dictionary<int, Tuple<string, AnswerDimension>> QuestionSelections { get; set; }
        public List<int> BlankIndices { get; set; }

        public QuestionContext()
        {
            this.QuestionSelections = new Dictionary<int, Tuple<string, AnswerDimension>>();
        }
    }

    public class SentenceParser
    {
        IParserRepository repositoryContext;

        public SentenceParser(IParserRepository repositoryContext)
        {
            this.repositoryContext = repositoryContext;
        }

        public void PopulateSentences(string filename, int topic)
        {
            IParserContext context = new ParserContext(this.repositoryContext);
            try
            {
                context.FileName = filename;
                context.PrimaryTopic = topic;
                context.LineNumber = 0;

                Topic primaryTopic = this.repositoryContext.GetTopicByIndex(topic);

                var lines = File.ReadAllLines(filename);
                foreach (var line in lines)
                {
                    ++context.LineNumber;
                    Sentence sentence = new Sentence();
                    sentence.PrimaryTopic = new TopicHandle(primaryTopic);
                    context.Sentence = sentence;

                    string currentColName = string.Empty;

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var tokens = line.Split("\t".ToCharArray());
                    context.ColIndex = 0;
                    foreach (var token in tokens)
                    {
                        context.ColIndex += token.Length + 4;
                        if (token.Trim() == "###")
                        {
                            continue;
                        }

                        // 1. if the token is a column name.
                        if (Columns.IsColumnName(token.Trim()))
                        {
                            if (token.Trim().Equals(Columns.WordTranslation)
                                && currentColName.Equals(Columns.GrammarEntries))
                            {
                                context.IsGrammarEntryTranslation = true;
                            }

                            currentColName = token.Trim();
                            context.CurrentColName = currentColName;
                            context.ValueIndex = -1;
                            continue;
                        }

                        // 2. else its a column value.
                        if (Columns.ShouldMaintainValueIndexCount(currentColName))
                        {
                            context.ValueIndex++;
                        }

                        context.CurrentColValue = token.Trim();
                        context = Columns.ProcessColumn(currentColName, context);

                        if (context.IsExistingSentence && !context.HasSentenceBeenCleared)
                        {
                            ClearSentenceFields(context.Sentence);
                            context.HasSentenceBeenCleared = true;
                        }
                    }

                    this.repositoryContext.SaveSentence(context.Sentence);
                    context.GrammarAnalysisContexts.Clear();
                    context.GrammarContexts.Clear();
                    context.IsGrammarEntryTranslation = false;
                    context.QuestionContexts.Clear();
                    context.VocabContexts.Clear();
                    context.ValueIndex = -1;
                    context.IsExistingSentence = false;
                    context.HasSentenceBeenCleared = false;
                }

            }
            catch (Exception ex)
            {
                throw new ParsingException(context, ex);
            }
        }

        private static void ClearSentenceFields(Sentence sentence)
        {
            // This method only clears the collection fields. It doesn't clear text, translation etc as those
            // are already replaced while parsing.

            sentence.ContextualAnalysis.Clear();
            sentence.GrammarAnalysis.Clear();
            sentence.VocabEntries.Clear();
            sentence.GrammarEntries.Clear();
            sentence.Questions.Clear();
            sentence.RoleConjugationPairs.Clear();
            sentence.SecondaryTopics.Clear();
            sentence.Tags.Clear();
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
            public const string Question6 = "Question6";
            public const string QuestionString = "QuestionString";
            public const string QuestionSubstring = "QuestionSubstring";
            public const string QuestionDimensionColumn = "QuestionDimension";
            public const string QuestionSelection01 = "QuestionSelection01";
            public const string QuestionSelection02 = "QuestionSelection02";
            public const string QuestionSelection03 = "QuestionSelection03";
            public const string QuestionSelection04 = "QuestionSelection04";
            public const string AnswerDimension01 = "AnswerDimension01";
            public const string AnswerDimension02 = "AnswerDimension02";
            public const string AnswerDimension03 = "AnswerDimension03";
            public const string AnswerDimension04 = "AnswerDimension04";
            public const string IncludedTopics = "IncludedTopics";
            public const string Tags = "Tags";
            public const string BlankIndex = "BlankIndex";

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
                Question6,
                QuestionString,
                QuestionSubstring,
                QuestionDimensionColumn,
                QuestionSelection01,
                QuestionSelection02,
                QuestionSelection03,
                QuestionSelection04,
                AnswerDimension01,
                AnswerDimension02,
                AnswerDimension03,
                AnswerDimension04,
                IncludedTopics,
                Tags,
                BlankIndex,
            };

            public static Func<IParserContext, IParserContext> EmptyParseFunc = ctx => ctx;
            public static Dictionary<string, Func<IParserContext, IParserContext>> ColumnProcessorMap =
                new Dictionary<string, Func<IParserContext, IParserContext>>() 
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
                { Question6, EmptyParseFunc },
                { QuestionString, ProcessQuestionString },
                { QuestionSubstring, ProcessQuestionSubstring },
                { QuestionDimensionColumn, ProcessQuestionDimension }, 
                { QuestionSelection01, ProcessQuestionSelection },
                { QuestionSelection02, ProcessQuestionSelection },
                { QuestionSelection03, ProcessQuestionSelection },
                { QuestionSelection04, ProcessQuestionSelection },
                { AnswerDimension01, ProcessAnswerDimension },
                { AnswerDimension02, ProcessAnswerDimension },
                { AnswerDimension03, ProcessAnswerDimension },
                { AnswerDimension04, ProcessAnswerDimension },
                { IncludedTopics, ProcessIncludedTopic },
                { Tags, ProcessTag },
                { BlankIndex, ProcessBlankIndex },
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

            public static IParserContext ProcessColumn(string colName, IParserContext context)
            {
                if (!ColumnProcessorMap.ContainsKey(colName))
                {
                    throw new InvalidOperationException(string.Format("invalid column name {0}", colName));
                }

                return ColumnProcessorMap[colName](context);
            }

            public static IParserContext ProcessSentenceText(IParserContext context)
            {
                context.Sentence.SentenceText = context.CurrentColValue;

                Sentence sFromRepo = context.RepositoryContext.GetSentenceByText(context.Sentence.SentenceText);

                if (sFromRepo != null)
                {
                    if (sFromRepo.PrimaryTopic.ObjectId.Equals(context.Sentence.PrimaryTopic.ObjectId))
                    {
                        context.Sentence = sFromRepo;
                        context.IsExistingSentence = true;
                    }
                }

                return context;
            }

            public static IParserContext ProcessSentenceTranslation(IParserContext context)
            {
                context.Sentence.SentenceTranslation = context.CurrentColValue;
                return context;
            }

            public static IParserContext ProcessVocabEntry(IParserContext context)
            {
                VocabEntryHandle handle = CreateVocabEntryIfNotExist(context.RepositoryContext, context.CurrentColValue);
                context.Sentence.VocabEntries.Add(handle);

                VocabContext vc = new VocabContext() { Text = context.CurrentColValue };
                context.VocabContexts.Add(context.ValueIndex, vc);

                return context;
            }

            public static IParserContext ProcessWordTranslation(IParserContext context)
            {
                if (!context.IsGrammarEntryTranslation)
                {
                    if (context.ValueIndex >= context.VocabContexts.Count())
                    {
                        // TODO: Throw a meaningful exception here.
                        ReportError("Value Index out of range when parsing vocab entry translation", context);
                    }

                    string vocabText = (context.VocabContexts[context.ValueIndex] as VocabContext).Text;
                    var results = context.RepositoryContext.GetVocabEntriesByText(vocabText);

                    VocabEntry ve = results.FirstOrDefault();
                    if (ve != null)
                    {
                        ve.Translation = context.CurrentColValue;
                        context.RepositoryContext.SaveVocabEntry(ve);
                    }
                    else
                    {
                        // TODO: Vocab entry not found, throw meaningful error
                        ReportError(string.Format("Vocab entry not found {0}, translation {1}", vocabText, context.CurrentColValue), context);
                    }

                    (context.VocabContexts[context.ValueIndex] as VocabContext).Translation = context.CurrentColValue;
                }
                else
                {
                    if (context.ValueIndex >= context.GrammarContexts.Count())
                    {
                        // TODO: Throw a meaningful exception here.
                        ReportError("Value Index out of range when parsing grammar entry translation", context);
                    }

                    string grammarEntryText = (context.GrammarContexts[context.ValueIndex] as GrammarEntryContext).Text;
                    var results = context.RepositoryContext.GetGrammarEntriesByText(grammarEntryText);

                    GrammarEntry ge = results.FirstOrDefault();
                    if (ge != null)
                    {
                        ge.Translation = context.CurrentColValue;
                        context.RepositoryContext.SaveGrammarEntry(ge);
                    }
                    else
                    {
                        // TODO: Vocab entry not found, throw meaningful error
                        ReportError(string.Format("Grammar entry not found {0}, translation {1}", grammarEntryText, context.CurrentColValue), context);
                    }

                    (context.GrammarContexts[context.ValueIndex] as GrammarEntryContext).Translation = context.CurrentColValue;

                }

                return context;
            }

            public static IParserContext ProcessConjugation(IParserContext context)
            {
                if (context.ValueIndex >= context.VocabContexts.Count())
                {
                    // TODO: Throw a meaningful exception here.
                    ReportError("Value Index out of range when parsing vocab entry conjugation", context);
                }

                string vocabText = (context.VocabContexts[context.ValueIndex] as VocabContext).Text;
                var results = context.RepositoryContext.GetVocabEntriesByText(vocabText);

                VocabEntry ve = results.FirstOrDefault();
                if (ve != null)
                {
                    ve.Conjugation = ParseConjugation(context.CurrentColValue);
                    context.RepositoryContext.SaveVocabEntry(ve);
                }
                else
                {
                    // TODO: Vocab entry not found, throw meaningful error
                    ReportError(string.Format("Vocab entry not found {0}, conjugation {1}", vocabText, context.CurrentColValue), context);
                }

                (context.VocabContexts[context.ValueIndex] as VocabContext).Conjugation = context.CurrentColValue;
                return context;
            }

            public static IParserContext ProcessGrammarEntry(IParserContext context)
            {
                GrammarEntryHandle handle = CreateGrammarEntryIfNotExist(context.RepositoryContext, context.CurrentColValue);
                context.Sentence.GrammarEntries.Add(handle);

                GrammarEntryContext gec = new GrammarEntryContext() { Text = context.CurrentColValue };
                context.GrammarContexts.Add(context.ValueIndex, gec);

                return context;
            }

            public static IParserContext ProcessGrammarRole(IParserContext context)
            {
                GrammarRoleHandle handle = CreateGrammarRoleIfNotExist(context.RepositoryContext, context.CurrentColValue);

                GrammarAnalysisContext gac = new GrammarAnalysisContext() { GrammarRole = context.CurrentColValue };
                context.GrammarAnalysisContexts.Add(context.ValueIndex, gac);

                return context;
            }

            public static IParserContext ProcessGrammaticalAnalysisElement(IParserContext context)
            {
                if (context.ValueIndex >= context.GrammarAnalysisContexts.Count())
                {
                    // TODO: Throw a meaningful exception here.
                    ReportError("Value Index out of range when parsing grammar role analysis entry", context);
                }

                string roleText = (context.GrammarAnalysisContexts[context.ValueIndex] as GrammarAnalysisContext).GrammarRole;
                var results = context.RepositoryContext.GetGrammarRolesByName(roleText);

                if (results.Count() <= 0)
                {
                    // TODO: Grammar Role not found, throw meaningful error
                    ReportError(string.Format("Grammar Role not found {0}", roleText), context);
                }

                var tuple = ParseArrows(context.CurrentColValue);

                GrammarAnalysisElement gae = new GrammarAnalysisElement();
                GrammarRoleHandle startingRoleHandle = null;
                if (tuple.Item1.Count == 1 && tuple.Item1[0] != 0)
                {
                    GrammarAnalysisContext gac = (context.GrammarAnalysisContexts[tuple.Item1[0] - 1] as GrammarAnalysisContext);
                    gac.AnalysisEntry = context.CurrentColValue;
                    string startingRoleText = gac.GrammarRole;
                    GrammarRole startingRole = context.RepositoryContext.GetGrammarRoleByName(startingRoleText);

                    if (startingRole == null)
                    {
                        ReportError(string.Format("Grammar Role not found {0}", startingRoleText), context);
                    }

                    startingRoleHandle = new GrammarRoleHandle(startingRole);
                }

                gae.StartSegmentRolePair = new QuantaeTuple<List<int>, GrammarRoleHandle>(tuple.Item1, startingRoleHandle);
                gae.EndSegmentRolePair = new QuantaeTuple<List<int>, GrammarRoleHandle>(
                    tuple.Item2,
                    new GrammarRoleHandle(results.First()));

                context.Sentence.GrammarAnalysis.Add(gae);

                return context;
            }

            public static IParserContext ProcessContextualAnalysisElement(IParserContext context)
            {
                string processedValue = context.CurrentColValue.Replace("\"", "").Trim();
                string[] indices = processedValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                List<int> list = new List<int>();
                foreach (var i in indices)
                {
                    int a = int.Parse(i.Trim());
                    list.Add(a);
                }

                context.Sentence.ContextualAnalysis.Add(list.ToArray());

                return context;
            }

            public static IParserContext ProcessRoleConjugationPair(IParserContext context)
            {
                string role = string.Empty;
                Conjugation conjugation = ParseRules(context.CurrentColValue, out role);
                role = role.Trim();
                if (!string.IsNullOrEmpty(role) && conjugation != null)
                {
                    CreateGrammarRoleIfNotExist(context.RepositoryContext, role);
                    GrammarRole gr = context.RepositoryContext.GetGrammarRoleByName(role);
                    QuantaeTuple<GrammarRoleHandle, DataModel.Conjugation> tuple =
                        new QuantaeTuple<GrammarRoleHandle, DataModel.Conjugation>(new GrammarRoleHandle(gr), conjugation);
                    context.Sentence.RoleConjugationPairs.Add(tuple);
                }

                return context;
            }

            public static IParserContext ProcessQuestionString(IParserContext context)
            {
                QuestionContext qc = new QuestionContext();
                qc.QuestionString = context.CurrentColValue;
                context.QuestionContexts.Add(context.QuestionContexts.Count, qc);
                return context;
            }

            public static IParserContext ProcessQuestionSubstring(IParserContext context)
            {
                QuestionContext qc = (QuestionContext)context.QuestionContexts[context.QuestionContexts.Count - 1];
                qc.QuestionSubstring = context.CurrentColValue;
                return context;
            }

            public static IParserContext ProcessQuestionDimension(IParserContext context)
            {
                QuestionContext qc = (QuestionContext)context.QuestionContexts[context.QuestionContexts.Count - 1];
                qc.Dimension = (QuestionDimension)Enum.Parse(typeof(QuestionDimension), context.CurrentColValue, true);
                // we now have enough information to insert this question into the sentence.

                Question q = new Question();
                q.QuestionString = qc.QuestionString;
                q.QuestionSubstring = qc.QuestionSubstring;
                q.Dimension = qc.Dimension;

                context.Sentence.Questions.Add(qc.Dimension, q);

                return context;
            }

            public static IParserContext ProcessBlankIndex(IParserContext context)
            {
                QuestionContext qc = (QuestionContext)context.QuestionContexts[context.QuestionContexts.Count - 1];
                qc.BlankIndices = ParseBlankIndices(context.CurrentColValue);
                context.Sentence.Questions[qc.Dimension].BlankPositions = qc.BlankIndices;
                return context;
            }

            public static IParserContext ProcessQuestionSelection(IParserContext context)
            {
                QuestionContext qc = (QuestionContext)context.QuestionContexts[context.QuestionContexts.Count - 1];
                Tuple<string, AnswerDimension> selection = Tuple.Create<string, AnswerDimension>(context.CurrentColValue, AnswerDimension.Unknown);
                int index = ExtractIndexFromColName(context.CurrentColName, "QuestionSelection");
                qc.QuestionSelections.Add(index, selection);
                AnswerChoice choice = new AnswerChoice();
                choice.Answer = context.CurrentColValue;
                context.Sentence.Questions[qc.Dimension].AnswerChoices.Add(choice);
                return context;
            }

            public static IParserContext ProcessAnswerDimension(IParserContext context)
            {
                if (string.IsNullOrWhiteSpace(context.CurrentColValue))
                {
                    return context;
                }

                QuestionContext qc = (QuestionContext)context.QuestionContexts[context.QuestionContexts.Count - 1];
                var answerDimension = (AnswerDimension)Enum.Parse(typeof(AnswerDimension), context.CurrentColValue, true);
                int index = ExtractIndexFromColName(context.CurrentColName, "AnswerDimension");
                string existingQuestionSelection = qc.QuestionSelections[index].Item1;
                qc.QuestionSelections[index] = Tuple.Create(existingQuestionSelection, answerDimension);

                context.Sentence.Questions[qc.Dimension].AnswerChoices[index - 1].Dimension = answerDimension;

                return context;
            }

            public static IParserContext ProcessIncludedTopic(IParserContext context)
            {
                int topicIndex = int.Parse(context.CurrentColValue.Trim());
                Topic t = context.RepositoryContext.GetTopicByIndex(topicIndex);
                if (t == null)
                {
                    ReportError(string.Format("Included Topic {0} not found.", topicIndex), context);
                }

                context.Sentence.SecondaryTopics.Add(new TopicHandle(t));
                return context;
            }

            public static IParserContext ProcessTag(IParserContext context)
            {
                context.Sentence.Tags.Add(context.CurrentColValue.Trim());
                return context;
            }

            private static VocabEntryHandle CreateVocabEntryIfNotExist(IParserRepository repositoryContext, string vocab)
            {
                var results = repositoryContext.GetVocabEntriesByText(vocab);

                VocabEntry entry = results.FirstOrDefault();
                if (entry == null)
                {
                    entry = new VocabEntry();
                    entry.Text = vocab;
                    repositoryContext.SaveVocabEntry(entry);
                }

                return new VocabEntryHandle(entry);
            }

            private static GrammarEntryHandle CreateGrammarEntryIfNotExist(IParserRepository repositoryContext, string grammarEntry)
            {
                var results = repositoryContext.GetGrammarEntriesByText(grammarEntry);

                GrammarEntry entry = results.FirstOrDefault();
                if (entry == null)
                {
                    entry = new GrammarEntry();
                    entry.Text = grammarEntry;
                    repositoryContext.SaveGrammarEntry(entry);
                }

                return new GrammarEntryHandle(entry);
            }

            private static GrammarRoleHandle CreateGrammarRoleIfNotExist(IParserRepository repositoryContext, string role)
            {
                var results = repositoryContext.GetGrammarRolesByName(role);

                GrammarRole gr = results.FirstOrDefault();
                if (gr == null)
                {
                    gr = new GrammarRole();
                    gr.RoleName = role;
                    repositoryContext.SaveGrammarRole(gr);
                }

                return new GrammarRoleHandle(gr);
            }

            private static Tuple<List<int>, List<int>> ParseArrows(string arrowsStr)
            {
                arrowsStr = arrowsStr.Replace("\"", "");
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
                string processed = colValue.Replace("\"", "").Replace(";", "");
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

            private static void ReportError(string message, IParserContext context)
            {

                string msg = string.Format("{0} @ {1}:{2},{3}", message, context.FileName, context.LineNumber, context.ColIndex);
                throw new InvalidDataException(msg);
            }

            private static Conjugation ParseRules(string rules, out string role)
            {
                role = string.Empty;
                if (string.IsNullOrWhiteSpace(rules))
                {
                    return null;
                }

                rules = rules.Replace("\"", "");

                string regex = @"\((?<role>.*?)\)\s*(?<conj>.*?);?";

                var matches = Regex.Matches(rules, regex);
                Match m = matches[0];
                role = m.Groups["role"].Value;

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

            private static int ExtractIndexFromColName(string colName, string prefix)
            {
                return int.Parse(colName.Replace(prefix, ""));
            }

            private static List<int> ParseBlankIndices(string colValue)
            {
                colValue = colValue.Replace("\"", "");
                var tokens = colValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                List<int> result = new List<int>();
                foreach (var token in tokens)
                {
                    result.Add(int.Parse(token));
                }

                return result;
            }
        }
    }
}
