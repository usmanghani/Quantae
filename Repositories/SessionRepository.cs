using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace Quantae.Repositories
{
    public class SessionRepository : IRepository<UserSession>
    {
        private SafeMode safeMode = SafeMode.True;
        public DataStore DataStore { get; set; }
        public string CollectionName { get; set; }
        public MongoCollection<UserSession> Collection { get; set; }
        public string[] Indices { get; set; }

        public SessionRepository(DataStore dataStore, params string[] indices)
        {
            this.DataStore = dataStore;
            this.CollectionName = "UserSessions";
            this.Collection = this.DataStore.GetCollection<UserSession>(this.CollectionName);
            this.Indices = indices;
            this.Collection.EnsureIndex(this.Indices);
        }

        public IEnumerable<UserSession> GetAllItems()
        {
            var cursor = this.Collection.FindAllAs<UserSession>();
            return cursor.AsEnumerable<UserSession>();
        }

        public long CountItems()
        {
            return this.Collection.Count();
        }

        public IEnumerable<UserSession> FindAs(IMongoQuery query, IMongoSortBy sortBy = null, int skip = -1, int limit = -1, int batchsize = -1, string indexHint = "", bool tableScan = false)
        {
            var cursor = this.Collection.FindAs<UserSession>(query);

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

        public UserSession FindOneAs(IMongoQuery query)
        {
            return this.Collection.FindOneAs<UserSession>(query);
        }

        public bool Update(IMongoQuery query, IMongoUpdate update, bool upsert = false, bool updateAllMatching = false)
        {
            UpdateFlags flags = upsert ? UpdateFlags.Upsert : UpdateFlags.None;
            flags |= updateAllMatching ? UpdateFlags.Multi : UpdateFlags.None;
            return this.Collection.Update(query, update, flags, safeMode).Ok;
        }

        public void Save(UserSession doc)
        {
            SafeModeResult result = this.Collection.Save<UserSession>(doc, safeMode);

            if (!result.Ok)
            {
                string errorMessage = result.HasLastErrorMessage ? result.LastErrorMessage : string.Empty;
                throw new ApplicationException(string.Format("Failed to save {0} to the database. Error Message: {1}", doc.ToJson(), errorMessage));
            }
        }

        public UserSession GetSessionByToken(string token)
        {
            return this.Collection.FindOneByIdAs<UserSession>(new BsonString(token));
        }

        public void Remove(string token)
        {
            this.Collection.Remove(Query.EQ("Token", new BsonString(token)));
        }

        public void Remove(IMongoQuery query)
        {
            this.Collection.Remove(query);
        }
    }
}
