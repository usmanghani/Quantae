using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class Utils
    {
        public static long GenerateULongQuantaeObjectId()
        {
            // Used to be: return ulong.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssfffff"));
            return DateTime.UtcNow.ToBinary();
        }
    }
}
