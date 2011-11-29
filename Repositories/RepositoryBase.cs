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
    public abstract class RepositoryBase<TObject> : IRepository<TObject> where TObject : QuantaeObject
    {
        protected SafeMode safeMode = SafeMode.True;
        public DataStore DataStore { get; protected set; }
        public string CollectionName { get; protected set; }
        public MongoCollection<TObject> Collection { get; protected set; }
        public string[] Indices { get; protected set; }

        public RepositoryBase(DataStore store, string collectionName, params string[] indices)
        {
            this.DataStore = store;
            this.CollectionName = collectionName;
            this.Collection = store.GetCollection<TObject>(collectionName);
            this.Indices = indices;
            this.Collection.EnsureIndex(this.Indices);
        }

        public IEnumerable<TObject> GetAllItems()
        {
            var cursor = this.Collection.FindAllAs<TObject>();
            return cursor.AsEnumerable<TObject>();
        }

        public long CountItems()
        {
            return this.Collection.Count();
        }

        public IEnumerable<TObject> FindAs(IMongoQuery query, IMongoSortBy sortBy = null, int skip = -1, int limit = -1, int batchsize = -1, string indexHint = "", bool tableScan = false)
        {
            var cursor = this.Collection.FindAs<TObject>(query);

            if (sortBy != null)
            {
                cursor.SetSortOrder(sortBy);
            }

            if (skip > -1)
            {
                cursor.SetSkip(skip);
            }

            if (limit > -1)
            {
                cursor.SetLimit(limit);
            }

            if (batchsize > -1)
            {
                cursor.SetBatchSize(batchsize);
            }

            if (!string.IsNullOrEmpty(indexHint))
            {
                cursor.SetHint(new BsonDocument(new BsonElement(indexHint, new BsonInt32(1))));
            }

            if (tableScan)
            {
                cursor.SetHint(new BsonDocument(new BsonElement("natural", new BsonInt32(1))));
            }

            return cursor;
        }

        public TObject FindOneAs(IMongoQuery query)
        {
            return this.Collection.FindOneAs<TObject>(query);
        }

        public bool Update(IMongoQuery query, IMongoUpdate update, bool upsert = false, bool updateAllMatching = false)
        {
            UpdateFlags flags = upsert ? UpdateFlags.Upsert : UpdateFlags.None;
            flags |= updateAllMatching ? UpdateFlags.Multi : UpdateFlags.None;
            return this.Collection.Update(query, update, flags, safeMode).Ok;
        }

        public void Save(TObject doc)
        {
            SafeModeResult result = this.Collection.Save<TObject>(doc, safeMode);

            if (!result.Ok)
            {
                string errorMessage = result.HasLastErrorMessage ? result.LastErrorMessage : string.Empty;
                throw new ApplicationException(string.Format("Failed to save {0} to the database. Error Message: {1}", doc.ToJson(), errorMessage));
            }
        }

        public TObject GetItemByHandle<HandleType>(HandleType handle) where HandleType : QuantaeObjectHandle<TObject>
        {
            return this.Collection.FindOneByIdAs<TObject>(new BsonObjectId(handle.ObjectId.Value));
        }

        public void Remove<HandleType>(HandleType handle) where HandleType : QuantaeObjectHandle<TObject>
        {
            this.Collection.Remove(Query.EQ("ObjectID", new BsonObjectId(handle.ObjectId.Value)));
        }

        public void Remove(IMongoQuery query)
        {
            this.Collection.Remove(query);
        }

        public void EnsureIndex(string[] fieldNames)
        {
            this.Collection.EnsureIndex(fieldNames);
        }

        public TObject FindById(string id)
        {
            return this.Collection.FindOneByIdAs<TObject>(new BsonObjectId(id));
        }
    }
}
