using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Quantae
{
    public class QuantaeObject<T>
    {
        [Key]
        [BsonId(IdGenerator = typeof(QuantaeObjectIdGenerator))]
        public T ObjectId { get; set; }
    }
}
