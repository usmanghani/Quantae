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
            var results = this.collection.FindAs<UserProfile>(Query.EQ("UserID", new BsonString(userid)));

            if (results.Count() > 1)
            {
                throw new DuplicateUserIdException(userid);
            }

            return results.ElementAt(0);
        }

        public void CreateUser(string userid, string email, string passwordHash, string facebookToken = "", string twitterToken = "")
        {
            UserProfile profile = new UserProfile() { UserID = userid, Email = email, PasswordHash = passwordHash, FacebookToken = facebookToken, TwitterToken = twitterToken };
            this.collection.Save<UserProfile>(profile);
        }

        public void UpdateUserEmail(string userId, string email)
        {
            this.collection.Update(Query.EQ("UserID", new BsonString(userId)), Update.Set("Email", new BsonString(email)));
        }
    }
}
