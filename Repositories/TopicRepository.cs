using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Repositories
{
    public class TopicRepository : RepositoryBase<Topic>
    {
        public TopicRepository(DataStore dataStore) : base(dataStore, "Topics") { }
    }
}
