using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson;

namespace Quantae.DataModel
{
    public class QuantaeObject
    {
        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        public BsonObjectId ObjectId { get; set; }
    }

    public class QuantaeObjectHandle<TObject> where TObject : QuantaeObject
    {
        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        public BsonObjectId ObjectId { get; set; }

        public QuantaeObjectHandle(TObject obj)
        {
            this.ObjectId = obj.ObjectId;
        }

        public override bool Equals(object obj)
        {
            return ObjectId.Equals((obj as QuantaeObjectHandle<TObject>).ObjectId);
        }

        public override int GetHashCode()
        {
            return ObjectId.GetHashCode();
        }
    }
}
