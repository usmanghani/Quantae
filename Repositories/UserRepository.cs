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
    }
}
