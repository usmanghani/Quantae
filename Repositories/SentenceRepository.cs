using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Repositories
{
    public class SentenceRepository : RepositoryBase<Sentence>
    {
        public SentenceRepository(DataStore dataStore) : base(dataStore, "Sentences") { }
    }
}
