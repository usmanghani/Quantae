using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.Repositories
{
    public class Repositories
    {
        public SentenceRepository Sentences { get; set; }
        public VocabRepository Vocabulary { get; set; }
        public GrammarRolesRepository GrammarRoles { get; set; }
        public UserRepository Users { get; set; }
        public TopicRepository Topics { get; set; }
    }
}
