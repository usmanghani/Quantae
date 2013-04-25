using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Repositories
{
    public class GrammarEntryRepository : RepositoryBase<GrammarEntry>
    {
        public GrammarEntryRepository(DataStore dataStore, params string[] indices) : base(dataStore, "GrammarEntries", indices) { }
    }
}
