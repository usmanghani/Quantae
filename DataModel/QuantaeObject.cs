using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Quantae.DataModel
{
    public class QuantaeObject
    {
        [BsonId(IdGenerator = typeof(BsonObjectIdGenerator))]
        public BsonObjectId ObjectId { get; set; }

        public override bool Equals(object obj)
        {
            var from = this.ObjectId;
            var to = ((QuantaeObject)obj).ObjectId;

            bool result = from.CompareTo(to) == 0;
            return result;
        }
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
            var from = this.ObjectId;
            var to = ((QuantaeObjectHandle<TObject>)obj).ObjectId;
            bool result = from.CompareTo(to) == 0;
            return result;
        }

        public override int GetHashCode()
        {
            return this.ObjectId.GetHashCode();
        }
    }
}