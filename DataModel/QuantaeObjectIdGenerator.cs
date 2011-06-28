using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization;

namespace Quantae.DataModel
{
    public class QuantaeObjectIdGenerator : IIdGenerator
    {
        public object GenerateId(object container, object document)
        {
            return Utils.GenerateQuantaeObjectId();
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
