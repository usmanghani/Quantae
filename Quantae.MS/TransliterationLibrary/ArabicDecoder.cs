namespace Quantae.Transliteration.Encoding
{
    public interface ArabicDecoder
    {

        /**
         * Decodes a plain text <code>string</code> into the internal
         * {@link org.jqurantree.arabic.ByteFormat} according to the encoding
         * scheme.
         * 
         * @param text
         *            the plain text <code>string</code> to decode
         * 
         * @return a <code>byte[]</code> array in the internal
         *         {@link org.jqurantree.arabic.ByteFormat}
         */
        byte[] Decode(string text);
    }
}