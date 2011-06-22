using System;
using System.Collections.Generic;


namespace Quantae.Transliteration
{

    /**
     * The <code>ArabicTextBuilder</code> class is used to construct
     * {@link org.jqurantree.arabic.ArabicText} by wrapping a mutable sequence of
     * {@link org.jqurantree.arabic.ArabicCharacter ArabicCharacters}. Since
     * {@link ArabicText} instances are immutable, a builder is required to
     * construct new instances. This class is analogous to the Java
     * <code>StringBuilder</code> class. The class manages an internal
     * <code>byte[]</code> buffer which grows on demand.
     * 
     * @author Kais Dukes
     */
    public class ArabicTextBuilder
    {

        private static int INITIAL_CAPACITY = 4;
        private byte[] buffer;
        private int characterCount;
        private int characterCapacity;

        /**
         * Creates a new Arabic text builder with the default initial capacity.
         */
        public ArabicTextBuilder()
            : this(INITIAL_CAPACITY)
        {

        }

        /**
         * Creates a new Arabic text builder with the specified character capacity.
         * 
         * @param characterCapacity
         *            the initial size of the <code>byte[]</code> buffer measured in
         *            characters. Each character will occupy 3 bytes in the buffer.
         */
        public ArabicTextBuilder(int characterCapacity)
        {
            this.characterCapacity = characterCapacity;
            buffer = new byte[characterCapacity * ByteFormat.CHARACTER_WIDTH];
        }

        /**
         * Appends a new character to the end of the buffer.
         * 
         * @param characterType
         *            the type of Arabic letter or Quranic symbol to append, such as
         *            <i>Alif</i> or <i>Ba</i>.
         */
        public void add(CharacterType characterType)
        {

            // Check if the buffer is not large enough to support a new character.
            if (characterCount >= characterCapacity)
            {

                // Copy the existing data into a new larger buffer.
                int newCapacity = characterCapacity * 2;
                byte[] newBuffer = new byte[newCapacity
                        * ByteFormat.CHARACTER_WIDTH];
                Array.Copy(buffer, 0, newBuffer, 0, characterCapacity * ByteFormat.CHARACTER_WIDTH);

                // Use the new buffer.
                buffer = newBuffer;
                characterCapacity = newCapacity;
            }

            // Add the character to the end of the buffer.
            int offset = characterCount * ByteFormat.CHARACTER_WIDTH;
            buffer[offset] = characterType != null ? (byte)characterType : ByteFormat.WHITESPACE;
            buffer[offset + 1] = 0;
            buffer[offset + 2] = 0;

            // Increment character count.
            characterCount++;
        }

        /**
         * Attaches a diacritic to the last character in the buffer.
         * 
         * @param diacriticType
         *            the type of diacritic to attach, such as <i>Fatha</i> or
         *            <i>Shadda</i>.
         */
        public void add(DiacriticType diacriticType)
        {

            // Get offset into buffer, based on last character.
            int offset = (characterCount - 1) * ByteFormat.CHARACTER_WIDTH;

            // Set the diacritic.
            ByteFormat.setDiacritic(buffer, offset, diacriticType);
        }

        /**
         * Appends a new character with attached diacritics to the end of the
         * buffer.
         * 
         * @param characterType
         *            the type of Arabic letter or Quranic symbol to append, such as
         *            <i>Alif</i> or <i>Ba</i>.
         * 
         * @param diaciritcTypes
         *            a list of diacritics to attach, such as <i>Fatha</i> or
         *            <i>Shadda</i>.
         */
        public void add(CharacterType characterType, IEnumerable<DiacriticType> diaciritcTypes)
        {

            // Add character.
            add(characterType);

            // Add diacritics.
            foreach (DiacriticType diacriticType in diaciritcTypes)
            {
                add(diacriticType);
            }
        }

        /**
         * Appends a space character to the end of the buffer.
         */
        public void addWhitespace()
        {

            // A NULL character type maps to whitespace.
            add(CharacterType.Unknown);
        }

        /**
         * Converts the Arabic text builder to a <code>string</code>. The returned
         * string will represent the characters in the buffer using Unicode
         * encoding.
         * 
         * @return a string representation of the characters in the buffer using
         *         Unicode encoding
         */

        public override string ToString()
        {
            return toText().ToString();
        }

        /**
         * Creates a new {@link ArabicText} instance from the characters in the
         * buffer.
         * 
         * @return a new {@link ArabicText} instance
         */
        public ArabicText toText()
        {
            return new ArabicText(toByteArray());
        }

        /**
         * Gets a copy of the internal <code>byte[]</code> buffer holding only used
         * characters.
         * 
         * @return a <code>byte[]</code> array
         */
        public byte[] toByteArray()
        {

            // Initiate.
            byte[] buffer = this.buffer;
            int byteCount = characterCount * ByteFormat.CHARACTER_WIDTH;

            // Copy character buffer into a new buffer of correct size.
            if (byteCount != buffer.Length)
            {
                buffer = new byte[byteCount];
                Array.Copy(this.buffer, 0, buffer, 0, byteCount);
            }

            // Return buffer.
            return buffer;
        }
    }
}
