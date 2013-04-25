using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.Transliteration.Encoding
{
    public class BuckwalterDecoder : ArabicDecoderBase
    {
        /**
         * Creates a new Buckwalter decoder.
         */
        public BuckwalterDecoder()
            : base(BuckwalterTable.getBuckwalterTable())
        {
        }
    }
}
