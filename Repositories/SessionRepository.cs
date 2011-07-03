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
    public class SessionRepository
    {
        public DataStore DataStore { get; set; }
        public string CollectionName { get; set; }
        public MongoCollection<UserSession> Collection { get; set; }

        public SessionRepository(DataStore dataStore)
        {
            this.DataStore = dataStore;
            this.CollectionName = "UserSessions";
            this.Collection = this.DataStore.GetCollection<UserSession>(this.CollectionName);
        }


        public UserSession GetSessionByToken(string token)
        {
            return this.Collection.FindOneByIdAs<UserSession>(new BsonString(token));
        }

        public void Save(UserSession doc)
        {
            SafeModeResult result = this.Collection.Save<UserSession>(doc, SafeMode.True);

            if (!result.Ok)
            {
                string errorMessage = result.HasLastErrorMessage ? result.LastErrorMessage : string.Empty;
                throw new ApplicationException(string.Format("Failed to save {0} to the database. Error Message: {1}", doc.ToJson(), errorMessage));
            }
        }

        public IEnumerable<UserSession> GetAllItems()
        {
            var cursor = this.Collection.FindAllAs<UserSession>();
            return cursor.AsEnumerable<UserSession>();
        }

        public void Remove(string token)
        {
            this.Collection.Remove(Query.EQ("Token", new BsonString(token)));
        }

        public int CountItems()
        {
            return this.Collection.Count();
        }
    }
}
