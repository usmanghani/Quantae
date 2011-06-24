using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Conventions;

namespace Quantae.DataModel
{
    public class QuantaeObjectIdMemberConvention : IIdMemberConvention
    {
        public string FindIdMember(Type type)
        {
            foreach (var property in type.GetProperties())
            {
                if (property.Name.EndsWith("Id"))
                {
                    return property.Name;
                }
            }
            return null;
        }
    }
}
