
namespace Quantae.Transliteration.Encoding
{
    public interface ArabicEncoder
    {

        /**
         * Encodes the internal {@link org.jqurantree.arabic.ByteFormat} into plain
         * text according to the encoding scheme.
         * 
         * @param buffer
         *            the <code>byte[]</code> array to encode in the internal
         *            {@link org.jqurantree.arabic.ByteFormat}
         * 
         * @param offset
         *            the starting offset in the buffer
         * 
         * @param characterCount
         *            the number of characters to encode. Each character is
         *            represented by 3 bytes in the buffer.
         * 
         * @return a plain text <code>string</code>
         */
        string Encode(byte[] buffer, int offset, int characterCount, EncodingOptions options);
    }
}
