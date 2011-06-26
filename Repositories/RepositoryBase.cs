using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using Quantae.DataModel;
using MongoDB.Bson;

namespace Quantae.Repositories
{
    public abstract class RepositoryBase<T>
    {
        protected DataStore dataStore = null;
        protected string collectionName = string.Empty;
        protected MongoCollection<T> collection = null;

        public RepositoryBase(DataStore store, string collectionName)
        {
            this.dataStore = store;
            this.collectionName = collectionName;
            this.collection = store.GetCollection<T>(collectionName);
        }

        public T GetItemByHandle<HandleType>(HandleType handle) where HandleType : QuantaeObjectHandle<long>
        {
            return this.collection.FindOneByIdAs<T>(new BsonInt64(handle.ObjectId));
        }

        public void Save<T>(T doc)
        {
            this.collection.Save<T>(doc);
        }
    }
}
