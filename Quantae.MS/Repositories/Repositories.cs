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
        public static RepositoryBase<GrammarEntry> GrammarEntries { get; set; }
        public static RepositoryBase<GrammarRole> GrammarRoles { get; set; }
        public static RepositoryBase<UserProfile> Users { get; set; }
        public static RepositoryBase<Topic> Topics { get; set; }
        public static IRepository<UserSession> Sessions { get; set; }

        public static void Init(DataStore dataStore)
        {
            Sentences = new SentenceRepository(dataStore, "SentenceText");
            Vocabulary = new VocabRepository(dataStore, "Text");
            GrammarEntries = new GrammarEntryRepository(dataStore, "Text");
            GrammarRoles = new GrammarRolesRepository(dataStore, "RoleName");
            Users = new UserRepository(dataStore, "UserID", "Email");
            Topics = new TopicRepository(dataStore, "Index", "ToicName");
            Sessions = new SessionRepository(dataStore, "UserID");
        }
    }
}
