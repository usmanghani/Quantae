using System;
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

        public Topic GetTopicByIndex(int index)
        {
            return this.collection.FindOneAs<Topic>(Query.EQ("Index", new BsonInt32(index)));
        }

        public Topic GetTopicByName(string name)
        {
            return this.collection.FindOneAs<Topic>(Query.EQ("TopicName", new BsonString(name)));
        }

        public bool TopicExists(int index)
        {
            return GetTopicByIndex(index) != null;
        }

        public bool TopicExists(string name)
        {
            return GetTopicByName(name) != null;
        }
    }
}
