using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;

namespace Quantae
{
    public class QuantaeObjectIdGenerator : IIdGenerator
    {
        public object GenerateId(object container, object document)
        {
            return Utils.GenerateULongQuantaeObjectId();
        }

        public bool IsEmpty(object id)
        {
            if (id == null)
            {
                return true;
            }

            var actualId = (ulong)id;
            return actualId == 0;
        }
    }
}
