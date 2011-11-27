using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.IO;
using Quantae.Engine;
using Quantae.DataModel;
using Quantae.Repositories;
using MongoDB.Driver.Builders;
using FluentCassandra.Types;
using System.Configuration;
using Microsoft.Office.Interop.Excel;
using Quantae.ParserLibrary;

namespace Quantae
{
    class Program
    {
        static void Main(string[] args)
        {
            QuantaeEngine.Init(ConfigurationManager.AppSettings["MONGOHQ_DB"], ConfigurationManager.AppSettings["MONGOHQ_URL"]);

            //Topic firstTopic = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(1));
            //var sentences = Repositories.Repositories.Sentences.FindAs(SentenceQueries.GetSentencesByTopic(new TopicHandle(firstTopic)));
            //Console.WriteLine(sentences.Count());
            //Environment.Exit(1);

            if (!Directory.Exists(@"C:\tmp"))
            {
                Directory.CreateDirectory(@"c:\tmp");
            }

            MongoParserRepository repository = new MongoParserRepository();
            TopicsParser topicsParser = new TopicsParser(repository);

            var excelApp = new Application();
            //excelApp.Visible = true;

            Workbook workBook = excelApp.Workbooks.Open(@"C:\Dropbox\Database\Grammar Topic - Dependency Graph.xlsx", UpdateLinks: 3);

            workBook.RunAutoMacros(XlRunAutoMacro.xlAutoOpen);
            workBook.Activate();
            var outputSheet = (Worksheet)workBook.Sheets.get_Item("Output");
            outputSheet.Activate();

            string filename = @"c:\graph.txt";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            try
            {
                outputSheet.SaveAs(Filename: filename, FileFormat: XlFileFormat.xlUnicodeText);
            }
            catch
            {
            }

            workBook.Close(SaveChanges: false);

            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

            topicsParser.PopulateTopics(filename);

            ///// EXPORT FROM EXCEL
            var files = Directory.GetFiles(@"C:\Dropbox\Database\Topic Excel Sheets", "Database - Topic*xlsm");

            excelApp = new Application();
            //excelApp.Visible = true;
            int i = 1;
            foreach (var file in files)
            {
                workBook = excelApp.Workbooks.Open(file, UpdateLinks: 3);
                workBook.RunAutoMacros(XlRunAutoMacro.xlAutoOpen);
                workBook.Activate();
                outputSheet = (Worksheet)workBook.Sheets.get_Item("Output");
                outputSheet.Activate();
                filename = string.Format("c:\\tmp\\topic{0}.txt", i.ToString("D3"));
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }

                try
                {
                    outputSheet.SaveAs(Filename: filename, FileFormat: XlFileFormat.xlUnicodeText);
                }
                catch
                {
                }

                workBook.Close(SaveChanges: false);
                i++;
            }

            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            /////// EXPORT FROM EXCEL

            //// TRANSFORM TEXT FILES
            files = Directory.GetFiles(@"c:\\tmp", "topic*.txt");
            foreach (var file in files)
            {
                var lines = File.ReadAllLines(file);
                File.Delete(file);
                File.WriteAllLines(file, lines.Skip(1));
            }
            ///// TRANSFORM TEXT FILES

            HashSet<int> skippedTopics = new HashSet<int>();

            SentenceParser sentenceParser = new SentenceParser(repository);

            for (int topic = 1; topic <= 20; topic++)
            {
                if(skippedTopics.Contains(topic))
                {
                    continue;
                }

                filename = string.Format("c:\\tmp\\topic{0}.txt", topic.ToString("D3"));
                sentenceParser.PopulateSentences(filename, topic);
            }
        }

        private static void DoChains()
        {
            var topics = Repositories.Repositories.Topics.GetAllItems().OrderByDescending(t => t.Index);

            StreamWriter writer = new StreamWriter("d:\\chains.txt");

            Dictionary<int, int> countsByTopic = new Dictionary<int, int>();
            foreach (var topic in topics)
            {
                countsByTopic[topic.Index] = ShowChain(topic.Index, 0, writer);

                writer.WriteLine("===========");
                writer.WriteLine(countsByTopic[topic.Index].ToString());
                writer.WriteLine("==========");
            }

            writer.WriteLine(">>>>>>>>>>><<<<<<<<<<<<<<<<<");

            var topicsByCountSorted = countsByTopic.OrderByDescending(kvp => kvp.Value);
            foreach (var kvp in topicsByCountSorted)
            {
                writer.WriteLine(kvp.Key + " => " + kvp.Value);
            }

            writer.WriteLine(">>>>>>>>>>><<<<<<<<<<<<<<<");
            writer.Flush();
            writer.Close();
        }

        private static int ShowChain(int p, int indent, StreamWriter writer)
        {
            int sum = 0;
            var topic = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(p));

            writer.WriteLine(new string(' ', indent) + p);

            foreach (var d in topic.Dependencies)
            {
                var t = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(d));
                sum += ShowChain(t.Index, indent + 4, writer);
            }

            return ++sum;
        }

        private static Dictionary<int, Tuple<string, int, List<int>>> DoFwdLinks(Dictionary<int, Tuple<string, List<int>>> graph)
        {
            Dictionary<int, Tuple<string, List<int>>> graph2 = new Dictionary<int, Tuple<string, List<int>>>();

            foreach (var kvp in graph)
            {
                foreach (var dep in kvp.Value.Item2)
                {
                    if (!graph2.ContainsKey(dep) || graph2[dep] == null)
                    {
                        graph2[dep] = Tuple.Create(graph[dep].Item1, new List<int>());
                    }

                    graph2[dep].Item2.Add(kvp.Key);
                }
            }

            var sortedGraph = graph2.OrderBy(kvp => kvp.Key).ToDictionary(kvp => kvp.Key);

            Dictionary<int, Tuple<string, int, List<int>>> graph3 = new Dictionary<int, Tuple<string, int, List<int>>>();
            foreach (var g in graph)
            {
                if (sortedGraph.ContainsKey(g.Key))
                {
                    graph3.Add(g.Key, Tuple.Create(sortedGraph[g.Key].Value.Item1, sortedGraph[g.Key].Value.Item2.Count, sortedGraph[g.Key].Value.Item2));
                }
                else
                {
                    graph3.Add(g.Key, Tuple.Create(graph[g.Key].Item1, 0, new List<int>()));
                }
            }

            return graph3;
        }

        private static Dictionary<int, string> GetAllPossibleTopics(Dictionary<int, Tuple<string, List<int>>> depGraph, Dictionary<int, bool> topicsCovered)
        {
            Dictionary<int, string> possibles = new Dictionary<int, string>();

            var failed = topicsCovered.Where(kvp => !kvp.Value);
            foreach (var f in failed)
            {
                if (depGraph[f.Key].Item2.All(t => topicsCovered.Contains(new KeyValuePair<int, bool>(t, true))))
                {
                    possibles.Add(f.Key, depGraph[f.Key].Item1);
                }
            }

            foreach (var topic in depGraph.Keys)
            {
                if (!topicsCovered.ContainsKey(topic))
                {
                    if (depGraph[topic].Item2.All(t => topicsCovered.Contains(new KeyValuePair<int, bool>(t, true))))
                    {
                        possibles.Add(topic, depGraph[topic].Item1);
                    }
                }
            }

            return possibles;
        }

        private static Dictionary<int, Tuple<string, List<int>>> ReadDependencyGraph(string filename)
        {
            Dictionary<int, Tuple<string, List<int>>> graph = new Dictionary<int, Tuple<string, List<int>>>();

            var contents = File.ReadAllText(filename);
            var lines = contents.Split("\n\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var tokens = line.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                int topicId = int.Parse(tokens[0]);
                string topicName = tokens[1];

                List<int> dependencies = new List<int>();
                foreach (var token in tokens.Skip(2))
                {
                    string t = token.Trim();
                    if (string.IsNullOrEmpty(t))
                    {
                        continue;
                    }

                    dependencies.Add(int.Parse(token));
                }

                graph[topicId] = Tuple.Create<string, List<int>>(topicName, dependencies);
            }

            return graph;
        }

        private static bool IsUserValid(Dictionary<int, Tuple<string, List<int>>> topicGraph, Dictionary<int, bool> userProfile, out List<int> violations)
        {
            violations = new List<int>();
            bool result = true;

            foreach (var kvp in userProfile)
            {
                if (!kvp.Value && topicGraph[kvp.Key].Item2.Any(t => userProfile.ContainsKey(t) && !userProfile[t]))
                {
                    violations.Add(kvp.Key);
                    result = false;
                }
            }

            return result;
        }

        private static void EnumerateTopics()
        {
            var topics = Repositories.Repositories.Topics.GetAllItems().OrderBy(t => t.Index);
            using (StreamWriter writer = new StreamWriter(new FileStream("d:\\testingit.txt", FileMode.Create)))
            {
                foreach (var topic in topics)
                {
                    writer.WriteLine(topic.ToJson());
                }
            }

            topics = Repositories.Repositories.Topics.GetAllItems().OrderBy(t => t.Index);
            using (StreamWriter writer = new StreamWriter(new FileStream("d:\\testingit2.txt", FileMode.Create)))
            {
                foreach (var topic in topics)
                {
                    int index = topic.Index;
                    string name = topic.TopicName;
                    List<int> deps = new List<int>();
                    foreach (var d in topic.Dependencies)
                    {
                        Topic t = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(d));
                        deps.Add(t.Index);
                    }

                    List<int> fwdLinks = new List<int>();
                    foreach (var f in topic.ForwardLinks)
                    {
                        Topic t = Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(f));
                        fwdLinks.Add(t.Index);
                    }

                    writer.Write(index.ToString());
                    writer.Write("\t");
                    writer.Write(name.ToString());
                    writer.Write("\t");

                    foreach (var d in deps)
                    {
                        writer.Write(d);
                        writer.Write(",");
                    }

                    writer.Write("\t");

                    foreach (var f in fwdLinks)
                    {
                        writer.Write(f);
                        writer.Write(",");
                    }

                    writer.WriteLine();
                }
            }

            var grammarRoles = Repositories.Repositories.GrammarRoles.GetAllItems();
            using (StreamWriter writer = new StreamWriter(new FileStream("d:\\testingit3.txt", FileMode.Create)))
            {
                foreach (var grammarRole in grammarRoles)
                {
                    writer.WriteLine(grammarRole.ToJson());
                }
            }

            grammarRoles = Repositories.Repositories.GrammarRoles.GetAllItems();
            using (StreamWriter writer = new StreamWriter(new FileStream("d:\\testingit4.txt", FileMode.Create)))
            {
                foreach (var grammarRole in grammarRoles)
                {
                    writer.WriteLine(grammarRole.RoleName);
                }
            }
        }

        private static void DoQuantaeDataModelStuff()
        {
            Repositories.Repositories.Users.Save(new UserProfile() { UserID = "usman", Email = "usman.ghani@gmail.com" });
            UserProfile up = Repositories.Repositories.Users.FindOneAs(UserProfileQueries.GetUserByUserName("usman"));
            Console.WriteLine(up.UserID + "=>" + up.Email);
            Repositories.Repositories.Users.Update(UserProfileQueries.GetUserByUserName("usman"), UserProfileQueries.UpdateUserEmailUpdate("blahingham25@yahoo.com"));
            up = Repositories.Repositories.Users.FindOneAs(UserProfileQueries.GetUserByUserName("usman"));
            Console.WriteLine(up.UserID + "=>" + up.Email);
        }


        private static void DoTransliterationStuff()
        {
            string inputString = "محمد";
            //byte[] buffer = System.Text.Encoding.Unicode.GetBytes(inputString);
            string another = "muHamadN";
            var encoder = new Quantae.Transliteration.Encoding.BuckwalterEncoder();
            string encoded = encoder.Encode(inputString);
            string decoded = encoder.Decode(encoded);
            Console.WriteLine(encoded);
            Console.WriteLine(decoded);
            Console.WriteLine(inputString.Equals(decoded));
            System.IO.File.WriteAllText("c:\\blah", decoded);
            System.IO.File.WriteAllText("c:\\blah2", encoder.Decode(another));
        }

        private static UserProfile CreateUserTopicHistory(Dictionary<int, bool> topicHistory)
        {
            UserProfile profile = new UserProfile();

            foreach (var item in topicHistory.Keys)
            {
                TopicHistoryItem thi = new TopicHistoryItem();
                thi.IsSuccessful = topicHistory[item];
                thi.Topic = new TopicHandle(Repositories.Repositories.Topics.FindOneAs(TopicQueries.GetTopicByIndex(item)));
                profile.History.TopicHistory.Insert(0, thi);
            }

            return profile;
        }

        private static void DoGraphStuff()
        {

            var graph = ReadDependencyGraph(@"d:\downloads\dependencies.csv");

            foreach (var node in graph)
            {
                Console.WriteLine("{0}, {1}, {2}", node.Key, node.Value.Item1, string.Join(",", node.Value.Item2.ToArray()));
            }

            Console.WriteLine("----------------------------");

            var fwdLinks = DoFwdLinks(graph);

            foreach (var f in fwdLinks)
            {
                Console.WriteLine("{0}, {1}, {2}, {3}", f.Key, f.Value.Item2, f.Value.Item1, string.Join(",", f.Value.Item3.ToArray()));
            }

            Console.WriteLine("----------------------------");

            var orderedFwdLinksByPriority = fwdLinks.OrderBy(kvp => kvp.Value.Item2);

            foreach (var f in orderedFwdLinksByPriority)
            {
                Console.WriteLine("{0}, {1}, {2}, {3}", f.Key, f.Value.Item2, f.Value.Item1, string.Join(",", f.Value.Item3.ToArray()));
            }

            var possibles = GetAllPossibleTopics(graph, userTopicHistory);

            Console.WriteLine("----------------------------");

            foreach (var kvp in possibles)
            {
                Console.WriteLine(kvp.Key);
            }
        }

        private static void CheckUserValidity()
        {
            var graph = ReadDependencyGraph(@"d:\downloads\dependencies.csv");
            List<int> violations;
            bool isUserValid = IsUserValid(graph, userTopicHistory, out violations);
            Console.WriteLine("{0} -> {1}", isUserValid, string.Join(",", violations.Select(i => i.ToString()).ToArray()));

            isUserValid = IsUserValid(graph, userTopicHistory2, out violations);
            Console.WriteLine("{0} -> {1}", isUserValid, string.Join(",", violations.Select(i => i.ToString()).ToArray()));
        }

        private static Dictionary<int, bool> userTopicHistory = new Dictionary<int, bool>() { 
                {1, true}, {2, true}, {3, true}, {4, true}, {5, true}, {6, true},
                {7, true}, {8, false}, {10, true}, {11, true}, {12, true}, {13, false},
                {15, true}, {16, true}, {17, true}, {18, true}, {19, true}, {20, true},
                {21, false}, {22, true}, {23, true}, {24, true}, {25, true}, {26, true},
                {27, true}, {28, true}, {29, true}, {30, true}, {31, true}, {32, false},
                {33, false}, {34, true}, {35, true}, {36, true}, {37, true}
            };

        private static Dictionary<int, bool> userTopicHistory2 = new Dictionary<int, bool>() { 
                {1, true}, {2, true}, {3, true}, {4, true}, {5, true}, {6, true},
                {7, true}, {8, false}, {10, true}, {11, true}, {12, true}, {13, false},
                {15, true}, {16, true}, {17, true}, {18, true}, {19, true}, {20, true}
            };

        private static Dictionary<int, bool> userTopicHistory3 = new Dictionary<int, bool>()
        {
            {1, true}, {2, true}, {3, true}, {4, true}, {5, true}, {6, false}, {14, true}
        };

        private static Dictionary<int, bool> userTopicHistory4 = new Dictionary<int, bool>()
        {
            {1, true}, {2, true}, {3, true}, {4, true}, {5, true}, {6, false}, {14, false}
        };

        private static Dictionary<int, bool> userTopicHistory5 = new Dictionary<int, bool>()
        {
            {1, true}, {2, true}, {3, true}, {4, true}, {5, true}, {6, false}, {14, false}, {17, false}
        };

        private static Dictionary<int, bool> userTopicHistory6 = new Dictionary<int, bool>()
        {
            {1, true}, {2, true}, {3, true}, {4, true}, {5, true}, {6, false}, {14, false}, {17, true}
        };

        private static Dictionary<int, bool> userTopicHistory7 = new Dictionary<int, bool>()
        {
            {1, true}, {2, true}, {3, true}, {4, false}, {5, true}, {6, true}, {8, true}, {10, true}
        };

        private static Dictionary<int, bool> userTopicHistory8 = new Dictionary<int, bool>()
        {
            {1, false}, {3, true}, {6, true}, {8, true}, {10, true}, {12, true}, {13, true}, {14, true}, {17, true}, {18, true}, {19, true}, {20, true}, {21, true}, {22, true}
        };

        private static Dictionary<int, bool> userTopicHistory9 = new Dictionary<int, bool>()
        {
            {1, true}, {2, true}, {3, true}, {4, true}, {5, true}, {6, false}, {14, true}, {15, true}, {17, true}, {18, true}, {19, true}, {22, true}
        };

        private static Dictionary<int, bool> userTopicHistory10 = new Dictionary<int, bool>()
        {
            {1, true}, {2, true}, {3, true}, {4, true}, {5, true}, {6, false}, {7, true}, {8, true}, {9, true}, {10, true}, {11, true}, {12, true}, {13, true}, {14, true}, {15, true}, {16, true}, {17, true}, {18, true}, {19, true}, {20,true}, {21, true}, {22, true}, {23, false}
        };

        private static Dictionary<int, bool> userTopicHistory11 = new Dictionary<int, bool>()
        {
            {1, true}, {2, true}, {3, true}, {4, true}, {5, true}, {6, false}, {7, true}, {8, true}, {9, true}, {10, true}, {11, true}, {12, true}, {13, true}, {14, true}, {15, true}, {16, true}, {17, true}, {18, true}, {19, true}, {20,true}, {21, true}, {22, true}, {23, false}, {24, false}, {25, false}
        };

        private static Dictionary<int, bool> userTopicHistory12 = new Dictionary<int, bool>()
        {
            {1, true}, {2, true}, {3, true}, {4, true}, {5, true}, {6, false}, {7, true}, {8, true}, {9, true}, {10, true}, {11, true}, {12, true}, {13, true}, {14, true}, {15, true}, {16, true}, {17, true}, {18, true}, {19, true}, {20,true}, {21, true}, {22, true}, {23, true}, {24, true}, {25, true}
        };
    }
}
