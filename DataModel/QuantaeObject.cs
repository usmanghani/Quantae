using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace Quantae.DataModel
{
    public class QuantaeObject<T>
    {
        [BsonId(IdGenerator = typeof(QuantaeObjectIdGenerator))]
        public T ObjectId { get; set; }
    }

    public class QuantaeObjectHandle<T>
    {
        [BsonId(IdGenerator = typeof(QuantaeObjectIdGenerator))]
        public T ObjectId { get; set; }

        public override bool Equals(object obj)
        {
            return ObjectId.Equals((obj as QuantaeObjectHandle<T>).ObjectId);
        }

        public override int GetHashCode()
        {
            return ObjectId.GetHashCode();
        }
    }
}
