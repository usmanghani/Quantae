using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class Utils
    {
        public static ulong GenerateULongQuantaeObjectId()
        {
            return ulong.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssfffff"));
        }

        public static string GetHintFromAnswerDimension(AnswerDimension ad)
        {
            // TODO: Fill this up.

            return "USman";
        }
    }
}
