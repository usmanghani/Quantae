using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.Transliteration
{
    using Encoding;
    public class ArabicText
    {
        private byte[] buffer;
        private int offset;
        private int characterCount;

        public ArabicText(byte[] buffer)
        {
            this.buffer = buffer;
            this.offset = 0;
            this.characterCount = buffer.Length / ByteFormat.CHARACTER_WIDTH;
        }

        public override string ToString()
        {
            return new BuckwalterEncoder().Encode(buffer, offset, characterCount, EncodingOptions.None);
        }
    }
}
