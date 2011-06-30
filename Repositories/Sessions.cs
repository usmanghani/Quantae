using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace Quantae.Repositories
{
    public class SessionRepository : RepositoryBase<UserSession>
    {
        public SessionRepository(DataStore dataStore) : base(dataStore, "UserSessions") { }

        public UserSession GetSessionFromToken(string token)
        {
            var tokens = token.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length < 2)
            {
                return null;
            }

            var cursor = this.Collection.FindAs<UserSession>(Query.EQ("Token", new BsonString(token)));

            return cursor.OrderByDescending(session => session.Timestamp).FirstOrDefault();
        }

        public void RemoveSessionIfExists(string token)
        {
            this.Collection.FindAndRemove(Query.EQ("Token", new BsonString(token)), SortBy.Null);
        }
    }
}
