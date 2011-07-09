﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace Quantae.Repositories
{
    public class TopicRepository : RepositoryBase<Topic>
    {
        public TopicRepository(DataStore dataStore) : base(dataStore, "Topics") { }
    }
}
