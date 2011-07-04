using MongoDB.Bson.Serialization;
using System;

namespace Quantae.DataModel.Sql
{
    public class QuantaeObjectIdGenerator : IIdGenerator
    {
        public object GenerateId(object container, object document)
        {
            return DateTime.UtcNow.ToBinary();
        }

        public bool IsEmpty(object id)
        {
            if (id == null)
            {
                return true;
            }

            var actualId = (long)id;
            return actualId == 0;
        }
    }
}