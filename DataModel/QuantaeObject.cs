using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Quantae
{
    public class QuantaeObject<T>
    {
        [BsonId(IdGenerator = typeof(QuantaeObjectIdGenerator))]
        public T ObjectId { get; set; }
    }
}
