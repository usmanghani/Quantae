using System;

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