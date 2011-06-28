using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.Repositories
{
    public static class Repositories
    {
        public static SentenceRepository Sentences { get; set; }
        public static VocabRepository Vocabulary { get; set; }
        public static GrammarRolesRepository GrammarRoles { get; set; }
        public static UserRepository Users { get; set; }
        public static TopicRepository Topics { get; set; }
        public static SessionRepository Sessions { get; set; }

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
