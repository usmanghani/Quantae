using System;
using MongoDB.Bson.Serialization.Conventions;

namespace Quantae.DataModel.Sql
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