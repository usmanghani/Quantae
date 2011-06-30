using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Quantae.DataModel
{
    public class Utils
    {
        public static long GenerateQuantaeObjectId()
        {
            // Used to be: return ulong.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssfffff"));
            return DateTime.UtcNow.ToBinary();
        }
    }
}
