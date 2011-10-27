using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization;
using Quantae.DataModel;

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
                MongoConnectionStringBuilder connectionStringBuilder = new MongoConnectionStringBuilder();
                connectionStringBuilder.DatabaseName = databaseName;
                connectionStringBuilder.Username = creds.Username;
                connectionStringBuilder.Password = creds.Password;
                connectionStringBuilder.Server = new MongoServerAddress(uri.Host, uri.Port);
                this.Server = MongoServer.Create(connectionStringBuilder);
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
            //BsonSerializer.RegisterSerializer(typeof(Tuple<GrammarRoleHandle, Conjugation>), null);

            BsonClassMap.RegisterClassMap<QuantaeTuple<GrammarRoleHandle, Conjugation>>(cm =>
            {
                cm.MapProperty(c => c.Item1);
                cm.MapProperty(c => c.Item2);
            });

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

    //public class TupleDeserializer : IBsonSerializer
    //{
    //    public object Deserialize(MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
    //    {
    //        bsonReader.
    //        throw new NotImplementedException();
    //    }

    //    public object Deserialize(MongoDB.Bson.IO.BsonReader bsonReader, Type nominalType, IBsonSerializationOptions options)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public bool GetDocumentId(object document, out object id, out Type idNominalType, out IIdGenerator idGenerator)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void Serialize(MongoDB.Bson.IO.BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public void SetDocumentId(object document, object id)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

}
