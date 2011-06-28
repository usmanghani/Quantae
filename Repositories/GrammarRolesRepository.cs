using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace Quantae.Repositories
{
    public class GrammarRolesRepository : RepositoryBase<GrammarRole>
    {
        public GrammarRolesRepository(DataStore dataStore) : base(dataStore, "GrammarRoles") { }

        public IEnumerable<GrammarRole> GetGrammarRolesByName(string name)
        {
            var cursor = this.collection.FindAs<GrammarRole>(Query.EQ("RoleName", new BsonString(name)));
            return cursor.AsEnumerable();
        }

        public bool GrammarRoleExists(string name)
        {
            return this.collection.FindOneAs<GrammarRole>(Query.EQ("RoleName", new BsonString(name))) != null;
        }
    }
}
