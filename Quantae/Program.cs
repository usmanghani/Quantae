using System;
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

namespace Quantae
{
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

    class Program
    {
        static void DoWorkflowStuff()
        {
            MyStateMachine statemachine = new MyStateMachine();
            using (WorkflowRuntime runtime = new WorkflowRuntime())
            {
                runtime.StartRuntime();
                WorkflowInstance instance = runtime.CreateWorkflow(typeof(MyStateMachine));
                instance.Start();
            }
        }

        static void Main(string[] args)
        {
            DoWorkflowStuff();
            return;

            //string id = DateTime.UtcNow.ToString("yyyyMMddHHmmssfffff");
            //Console.WriteLine(id);
            //ulong idAsLong = ulong.Parse(id);
            //Console.WriteLine(idAsLong);
            //Console.WriteLine(idAsLong.ToString().CompareTo(id));

            //var myConventions = new ConventionProfile();
            //myConventions.SetIdMemberConvention(new QuantaeObjectIdMemberConvention());
            //BsonClassMap.RegisterConventions(myConventions, t => t.FullName.StartsWith("Quantae."));
            //BsonClassMap.RegisterClassMap<Sentence>();

            //var server = MongoServer.Create();
            //server.Connect();

            //var database = server["mydb"];
            //var collection = database["things"];

            //collection.Remove(new QueryDocument(new Dictionary<string, string>() { { "_id", "/^4d/" } }));

            //QueryDocument query = new QueryDocument(new Dictionary<string, string>() { { "_id", "201106200033439986" } });

            //Console.WriteLine(collection.Find(query).Count());
            //Console.WriteLine(collection.Count());

            //foreach (var doc in collection.FindAll())
            //{
            //    Console.WriteLine(doc.ToJson());
            //}

            //var id1 = Utils.GenerateULongQuantaeObjectId();

            //Console.WriteLine(collection.Save<Sentence>(new Sentence() { ObjectId = id1 }));

            //QueryDocument query2 = new QueryDocument(new Dictionary<string, string>() { { "_id", string.Format("NumberLong({0})", id1.ToString()) } });

            //Console.WriteLine(collection.Find(query).Count());

            //Console.WriteLine(collection.Count());

            //string inputString = "محمد";
            ////byte[] buffer = System.Text.Encoding.Unicode.GetBytes(inputString);
            //string another = "muHamadN";
            //var encoder = new Quantae.Transliteration.Encoding.BuckwalterEncoder();
            //string encoded = encoder.Encode(inputString);
            //string decoded = encoder.Decode(encoded);
            //Console.WriteLine(encoded);
            //Console.WriteLine(decoded);
            //Console.WriteLine(inputString.Equals(decoded));
            //System.IO.File.WriteAllText("c:\\blah", decoded);
            //System.IO.File.WriteAllText("c:\\blah2", encoder.Decode(another));
        }
    }
}
