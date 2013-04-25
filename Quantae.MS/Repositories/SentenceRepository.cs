using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace Quantae.Repositories
{
    public class SentenceRepository : RepositoryBase<Sentence>
    {
        public SentenceRepository(DataStore dataStore, params string[] indices) : base(dataStore, "Sentences", indices) { }

        public IEnumerable<Sentence> GetSentencesByTopic(TopicHandle topicHandle, int skip = 0, int limit = 0)
        {
            var cursor = this.Collection.FindAs<Sentence>(Query.EQ("PrimaryTopic", new BsonObjectId(topicHandle.ObjectId.Value)));
            cursor.SetSkip(skip);
            if (limit > 0)
            {
                cursor.SetLimit(limit);
            }
            return cursor.AsEnumerable();
        }
    }
}
