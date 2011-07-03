using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Quantae.DataModel
{
    public class QuantaeObject
    {
        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        public BsonObjectId ObjectId { get; set; }
    }

    public class QuantaeObjectHandle<TObject> where TObject : QuantaeObject
    {
        public BsonObjectId ObjectId { get; set; }

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