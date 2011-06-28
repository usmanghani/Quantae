using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using Quantae.DataModel;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace Quantae.Repositories
{
    public abstract class RepositoryBase<TObject> where TObject : QuantaeObject<long>
    {
        protected DataStore dataStore = null;
        protected string collectionName = string.Empty;
        protected MongoCollection<TObject> collection = null;

        public RepositoryBase(DataStore store, string collectionName)
        {
            this.dataStore = store;
            this.collectionName = collectionName;
            this.collection = store.GetCollection<TObject>(collectionName);
        }

        public TObject GetItemByHandle<HandleType>(HandleType handle) where HandleType : QuantaeObjectHandle<long, TObject>
        {
            return this.collection.FindOneByIdAs<TObject>(new BsonInt64(handle.ObjectId));
        }

        public void Save(TObject doc)
        {
            this.collection.Save<TObject>(doc);
        }

        public IEnumerable<TObject> GetAllItems()
        {
            var cursor = this.collection.FindAllAs<TObject>();
            return cursor.AsEnumerable<TObject>();
        }

        public void Remove<HandleType>(HandleType handle) where HandleType : QuantaeObjectHandle<long, TObject>
        {
            this.collection.Remove(Query.EQ("ObjectID", new BsonInt64(handle.ObjectId)));
        }

        public int CountItems()
        {
            return this.collection.Count();
        }
    }
}
