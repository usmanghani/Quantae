using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.Transliteration.Encoding
{
    public enum EncodingOptions
    {
        None,

        /**
         * Specifies that an <i>Alif</i> <code>ArabicCharacter</code> with an
         * attached <i>Maddah</i> should be combined and encoded into a single
         * output character.
         * 
         */
        CombineAlifWithMaddah
    }

}
