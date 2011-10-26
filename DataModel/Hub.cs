using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Quantae.DataModel
{
    [BsonKnownTypes(typeof(ExtrasHub))]
    public abstract class Hub
    {
        public abstract List<HubAction> Actions { get; set; }
    }
}