using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace Quantae.DataModel.Sql
{
    public class QuantaeObject
    {
        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        [Key]
        public int ObjectId { get; set; }
    }

    public class QuantaeObjectHandle<TObject> where TObject : QuantaeObject
    {
        public int ObjectId { get; set; }

        public QuantaeObjectHandle(TObject obj)
        {
            this.ObjectId = obj.ObjectId;
        }

        public override bool Equals(object obj)
        {
            return this.ObjectId.Equals((obj as QuantaeObjectHandle<TObject>).ObjectId);
        }

        public override int GetHashCode()
        {
            return this.ObjectId.GetHashCode();
        }
    }
}