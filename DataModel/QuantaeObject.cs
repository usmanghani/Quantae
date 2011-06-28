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

    public class QuantaeObjectHandle<THandle, TObject> where TObject : QuantaeObject<THandle>
    {
        [BsonId(IdGenerator = typeof(QuantaeObjectIdGenerator))]
        public THandle ObjectId { get; set; }

        public QuantaeObjectHandle(TObject obj)
        {
            this.ObjectId = obj.ObjectId;
        }

        public override bool Equals(object obj)
        {
            return ObjectId.Equals((obj as QuantaeObjectHandle<THandle, TObject>).ObjectId);
        }

        public override int GetHashCode()
        {
            return ObjectId.GetHashCode();
        }
    }
}
