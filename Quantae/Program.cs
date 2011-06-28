﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using System.Workflow;
using System.Workflow.ComponentModel;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using Quantae.DataModel;
using System.IO;

namespace Quantae
{
    using Quantae.Engine;
    using Quantae.DataModel;
    using Quantae.Repositories;
    using System.Text.RegularExpressions;

    class MyStateMachine : StateMachineWorkflowActivity
    {
        public StateActivity state1 = new StateActivity("blah");
        public StateActivity state2 = new StateActivity("blah2");

        public MyStateMachine()
        {
            this.Activities.Add(state1);
            this.Activities.Add(state2);
            this.InitialStateName = state1.Name;
            this.CompletedStateName = state2.Name;
        }
    }

    class A { public string Aa = null; public ObjectId ID = ObjectId.GenerateNewId(); }
    class B : A { public string Bb = null;}
    class C : A { public string Cc = null;}

    class Program
    {
        static void Main(string[] args)
        {
            DataStore dataStore = new DataStore("QuantaeTestDb");
            dataStore.Connect();
            Repositories.Repositories.Init(dataStore);

            //var roles = Repositories.Repositories.GrammarRoles.GetGrammarRolesByName("Mufrad");
            //foreach (var r in roles)
            //{
            //    Console.WriteLine(r.ToJson());
            //}

            //TopicGraphOperations.PopulateTopics("d:\\downloads\\dependencies-parseable.txt");
            //TopicGraphOperations.CreateForwardLinks();
            //EnumerateTopics();
            CheckUserValidity();
            //DoQuantaeDataModelStuff();
            //DoMongoStuff();
            //DoGraphStuff();
            //DoTransliterationStuff();
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
            using (StreamWriter writer = new StreamWriter(File.OpenWrite("d:\\testingit.txt")))
            {
                foreach (var topic in topics)
                {
                    writer.WriteLine(topic.ToJson());
                }
            }

            topics = Repositories.Repositories.Topics.GetAllItems().OrderBy(t => t.Index);
            using (StreamWriter writer = new StreamWriter(File.OpenWrite("d:\\testingit2.txt")))
            {
                foreach (var topic in topics)
                {
                    int index = topic.Index;
                    string name = topic.TopicName;
                    List<int> deps = new List<int>();
                    foreach (var d in topic.Dependencies)
                    {
                        deps.Add(Repositories.Repositories.Topics.GetItemByHandle(d).Index);
                    }

                    writer.Write(index.ToString());
                    writer.Write("\t");
                    writer.Write(name.ToString());
                    writer.Write("\t");

                    foreach (var p in topic.RoleConjugationPairs)
                    {
                        var r = Repositories.Repositories.GrammarRoles.GetItemByHandle(p.Item1);
                        writer.Write(r.RoleName);

                        writer.Write(";");
                        writer.Write(p.Item2.Gender.ToString());
                        writer.Write(",");
                        writer.Write(p.Item2.Number.ToString());

                        if (p.Item2.GetType() == typeof(VerbConjugation))
                        {
                            writer.Write(",");
                            var v = p.Item2 as VerbConjugation;
                            writer.Write(v.Tense.ToString());
                            writer.Write(",");
                            writer.Write(v.Person.ToString());
                        }

                        writer.Write(";");
                    }

                    writer.Write("\t");

                    foreach (var d in deps)
                    {
                        writer.Write(d);
                        writer.Write(",");
                    }

                    writer.WriteLine();
                }
            }

            var grammarRoles = Repositories.Repositories.GrammarRoles.GetAllItems();
            using (StreamWriter writer = new StreamWriter(File.OpenWrite("d:\\testingit3.txt")))
            {
                foreach (var grammarRole in grammarRoles)
                {
                    writer.WriteLine(grammarRole.ToJson());
                }
            }

            grammarRoles = Repositories.Repositories.GrammarRoles.GetAllItems();
            using (StreamWriter writer = new StreamWriter(File.OpenWrite("d:\\testingit4.txt")))
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
            UserProfile up = Repositories.Repositories.Users.GetUserByUserName("usman");
            Console.WriteLine(up.UserID + "=>" + up.Email);
            Repositories.Repositories.Users.UpdateUserEmail("usman", "blahingham25@yahoo.com");
            up = Repositories.Repositories.Users.GetUserByUserName("usman");
            Console.WriteLine(up.UserID + "=>" + up.Email);
        }

        private static void DoWorkflowStuff()
        {
            MyStateMachine statemachine = new MyStateMachine();
            using (WorkflowRuntime runtime = new WorkflowRuntime())
            {
                runtime.StartRuntime();
                WorkflowInstance instance = runtime.CreateWorkflow(typeof(MyStateMachine));
                instance.Start();
            }
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

            foreach (var item in userTopicHistory2.Keys)
            {

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
    }
}
