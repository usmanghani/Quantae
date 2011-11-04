using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using Quantae.DataModel;

namespace Quantae.Repositories
{
    public class VocabRepository : RepositoryBase<VocabEntry>
    {
        public VocabRepository(DataStore dataStore, params string[] indices) : base(dataStore, "Vocabulary", indices) { }
    }
}
