using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using Quantae.DataModel;

namespace Quantae.Repositories
{
    public class UserProfileQueries
    {
        public static IMongoQuery GetUserByUserName(string username)
        {
            return Query.EQ("UserID", new BsonString(username));
        }

        public static IMongoQuery GetUserByEmail(string email)
        {
            return Query.EQ("Email", new BsonString(email));
        }

        public static IMongoUpdate UpdateUserEmailUpdate(string email)
        {
            return Update.Set("Email", new BsonString(email));
        }
    }

    public class TopicQueries
    {
        public static IMongoQuery GetTopicByName(string name)
        {
            return Query.EQ("TopicName", new BsonString(name));
        }

        public static IMongoQuery GetTopicByIndex(int index)
        {
            return Query.EQ("Index", new BsonInt32(index));
        }

        public static IMongoQuery GetIndependentTopicsGreaterThanIndex(int index)
        {
            return Query.And(Query.Size("Dependencies", 0), Query.GT("Index", index));
        }
    }

    public class GrammarRoleQueries
    {
        public static IMongoQuery GetGrammarRoleByName(string name)
        {
            return Query.EQ("RoleName", new BsonString(name));
        }
    }

    public class SessionQueries
    {
        public static IMongoQuery GetSessionByToken(string token)
        {
            return Query.EQ("Token", new BsonString(token));
        }
    }

    public class SentenceQueries
    {
        public static IMongoQuery GetSentencesByTopic(TopicHandle handle)
        {
            return Query.EQ("PrimaryTopic", new BsonObjectId(handle.ObjectId.Value));
        }
    }

    public class VocabQueries
    {
        public static IMongoQuery GetVocabEntryByText(string text)
        {
            return Query.EQ("Text", new BsonString(text));
        }
    }
}
