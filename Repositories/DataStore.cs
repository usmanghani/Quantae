using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization;

namespace Quantae.Repositories
{
    public class DataStore
    {
        public Uri Uri { get; set; }
        public string CurrentDbName { get; set; }

        public MongoServer Server { get; set; }
        public MongoDatabase CurrentDatabase { get; set; }

        public DataStore(string databaseName, Uri uri = null, MongoCredentials creds = null)
        {
            this.InitializeConventions();
            this.InitializeClassMaps();

            this.Uri = uri;
            this.CurrentDbName = databaseName;

            if (uri != null)
            {
                this.Server = MongoServer.Create(uri);
            }
            else
            {
                this.Server = MongoServer.Create();
            }

            this.CurrentDatabase = this.Server.GetDatabase(this.CurrentDbName);
        }

        public void Connect()
        {
            this.Server.Connect();
        }

        public void Disconnect()
        {
            this.Server.Disconnect();
        }

        public MongoCollection<T> GetCollection<T>(string collectionName)
        {
            return this.CurrentDatabase.GetCollection<T>(collectionName);
        }

        private void InitializeConventions()
        {
            var myConventions = new ConventionProfile();
            myConventions.SetIdMemberConvention(new Quantae.DataModel.QuantaeObjectIdMemberConvention());
            BsonClassMap.RegisterConventions(myConventions, t => t.FullName.StartsWith("Quantae."));
        }

        private void InitializeClassMaps()
        {
            BsonClassMap.RegisterClassMap<KeyValuePair<int, int>>(cm =>
            {
                cm.MapProperty(c => c.Key);
                cm.MapProperty(c => c.Value);
            });

            BsonClassMap.RegisterClassMap<Tuple<int, int>>(cm =>
            {
                cm.MapProperty(c => c.Item1);
                cm.MapProperty(c => c.Item2);
            });

            BsonClassMap.RegisterClassMap<Tuple<string, List<int>>>(cm =>
            {
                cm.MapProperty(c => c.Item1);
                cm.MapProperty(c => c.Item2);
            });

            BsonClassMap.RegisterClassMap<Tuple<string, int, List<int>>>(cm =>
            {
                cm.MapProperty(c => c.Item1);
                cm.MapProperty(c => c.Item2);
                cm.MapProperty(c => c.Item3);
            });

        }
    }
}
