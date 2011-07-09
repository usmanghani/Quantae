using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Repositories
{
    public static class Repositories
    {
        public static RepositoryBase<Sentence> Sentences { get; set; }
        public static RepositoryBase<VocabEntry> Vocabulary { get; set; }
        public static RepositoryBase<GrammarRole> GrammarRoles { get; set; }
        public static RepositoryBase<UserProfile> Users { get; set; }
        public static RepositoryBase<Topic> Topics { get; set; }
        public static IRepository<UserSession> Sessions { get; set; }

        public static void Init(DataStore dataStore)
        {
            Sentences = new SentenceRepository(dataStore);
            Vocabulary = new VocabRepository(dataStore);
            GrammarRoles = new GrammarRolesRepository(dataStore);
            Users = new UserRepository(dataStore);
            Topics = new TopicRepository(dataStore);
            Sessions = new SessionRepository(dataStore);
        }
    }
}
