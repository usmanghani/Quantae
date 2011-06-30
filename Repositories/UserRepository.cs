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
    public class UserRepository : RepositoryBase<UserProfile>
    {
        public UserRepository(DataStore dataStore) : base(dataStore, "UserProfiles") { }

        public UserProfile GetUserByUserName(string userid)
        {
            var results = this.Collection.FindAs<UserProfile>(Query.EQ("UserID", new BsonString(userid)));

            if (results.Count() > 1)
            {
                throw new DuplicateUserIdException(userid);
            }

            if (results.Count() <= 0)
            {
                return null;
            }

            return results.ElementAt(0);
        }

        public void CreateUser(string userid, string email, string passwordHash, string facebookToken = "", string twitterToken = "")
        {
            UserProfile profile = new UserProfile() { UserID = userid, Email = email, PasswordHash = passwordHash, FacebookToken = facebookToken, TwitterToken = twitterToken };
            this.Collection.Save<UserProfile>(profile);
        }

        public void UpdateUserEmail(string userId, string email)
        {
            this.Collection.Update(Query.EQ("UserID", new BsonString(userId)), Update.Set("Email", new BsonString(email)));
        }

        public UserProfile GetUserByEmail(string email)
        {
            var results = this.Collection.FindAs<UserProfile>(Query.EQ("Email", new BsonString(email)));
            if (results.Count() > 1)
            {
                throw new DuplicateUserIdException(email);
            }

            if (results.Count() <= 0)
            {
                return null;
            }

            return results.ElementAt(0);
        }
    }
}
