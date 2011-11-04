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

namespace Quantae
{
    class A { public string Aa = null; public ObjectId ID = ObjectId.GenerateNewId(); }
    class B : A { public string Bb = null;}
    class C : A { public string Cc = null;}

    //class MyStateMachine : StateMachineWorkflowActivity
    //{
    //    public StateActivity state1 = new StateActivity("blah");
    //    public StateActivity state2 = new StateActivity("blah2");

    //    public MyStateMachine()
    //    {
    //        this.Activities.Add(state1);
    //        this.Activities.Add(state2);
    //        this.InitialStateName = state1.Name;
    //        this.CompletedStateName = state2.Name;
    //    }
    //}


    class Program
    {
        static void Main(string[] args)
        {
            //using (var db = new Quantae.DataModel.Sql.QuantaeDbContext())
            //{
            //    var user = new Quantae.DataModel.Sql.UserProfile() { Email = "usman.ghani@gmail.com", ObjectId = 1 };
            //    db.Users.Add(user);
            //    int recordsAffected = db.SaveChanges();
            //    Console.WriteLine(recordsAffected);
            //}

            //Environment.Exit(1);

            //DoCassandraStuff();
            //var server = MongoServer.Create();
            //server.Connect();
            //var result = server.RunAdminCommand(new CommandDocument(new BsonElement("fsync", new BsonInt32(1))));

            //System.Diagnostics.TraceSource ts = new System.Diagnostics.TraceSource("QuantaeTestTraceSource", SourceLevels.All);

            //ts.TraceData(TraceEventType.Critical, 1, "none", "helooo", "format", new object[] { "arg1", "arg2" });
            //ts.TraceData(TraceEventType.Critical, 1, "none", "helooo");
            //ts.TraceData(TraceEventType.Critical, 1, "none", "helooo");
            //ts.TraceData(TraceEventType.Critical, 1, "none", "helooo");
            //ts.TraceData(TraceEventType.Critical, 1, "none", "helooo");

            //ts.Flush();
            //ts.Close();

            QuantaeEngine.Init("5836dfd5-24c0-407d-a7a0-50679a4dd2d9");
            //TopicGraphUtilities.PopulateTopics(@"C:\graph.txt");
            SentenceUtilities.PopulateSentences(@"c:\abc.txt", 1);

            //Repositories.Repositories.Users.Save(new UserProfile());

            //for (int i = 0; i < 100; i++)
            //{
            //    var cursor = Repositories.Repositories.Topics.Collection.FindAll();

            //    cursor.SetSkip(i).SetSortOrder(SortBy.Ascending("Index")).SetFields("Index");

            //    if (cursor.Size() > 0)
            //    {
            //        Console.WriteLine(cursor.Size() + "::" + cursor.ElementAt(0).Index);
            //    }
            //}

            //TopicGraphUtilities.PopulateTopics("d:\\downloads\\dependencies-parseable.txt");
            //TopicGraphUtilities.CreateForwardLinks();
            //EnumerateTopics();

            //UserProfile profile = CreateUserTopicHistory(userTopicHistory12);
            //UserProfile profile2 = Repositories.Repositories.Users.FindOneAs(UserProfileQueries.GetUserByUserName("usman.ghani"));
            //profile2.History.TopicHistory = profile.History.TopicHistory;
            //Repositories.Repositories.Users.Save(profile2);

            //TopicHandle topicHandle = TopicGraphOperations.GetNextTopic(profile);

            //if (topicHandle != null)
            //{
            //    Console.WriteLine(Repositories.Repositories.Topics.GetItemByHandle(topicHandle).Index);
            //}
            //else
            //{
            //    Console.WriteLine("STUCK");
            //}

            //DoChains();

            //Topic topic = Repositories.Repositories.Topics.GetItemByHandle(topicHandle);

            //if (topic == null)
            //{
            //    Console.WriteLine("<null>");
            //}
            //else
            //{
            //    Console.WriteLine(topic.Index + " -> " + topic.TopicName);
            //}

            //CheckUserValidity();
            //DoQuantaeDataModelStuff();
            //DoGraphStuff();
            //DoTransliterationStuff();
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

        private static void DoCassandraStuff()
        {
            using (var db = new FluentCassandra.CassandraContext(keyspace: "quantae", host: "localhost"))
            {
                var family = db.GetColumnFamily<UTF8Type, UTF8Type>("userprofiles");
            }
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

        //private static void DoWorkflowStuff()
        //{
        //    MyStateMachine statemachine = new MyStateMachine();
        //    using (WorkflowRuntime runtime = new WorkflowRuntime())
        //    {
        //        runtime.StartRuntime();
        //        WorkflowInstance instance = runtime.CreateWorkflow(typeof(MyStateMachine));
        //        instance.Start();
        //    }
        //}

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

        private static Quantae.DataModel.UserProfile CreateUserTopicHistory(Dictionary<int, bool> topicHistory)
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
