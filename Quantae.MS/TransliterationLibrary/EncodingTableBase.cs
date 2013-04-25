using System;
using System.Collections.Generic;

namespace Quantae.Transliteration.Encoding
{
    public abstract class EncodingTableBase
    {

        private Dictionary<char, EncodingTableItem> unicodeMap = new Dictionary<char, EncodingTableItem>();

        private char[] characterList = new char[Enum.GetValues(typeof(CharacterType)).Length];
        private char[] unicodeList = new char[Enum.GetValues(typeof(UnicodeType)).Length];

        /**
         * Creates a new encoding table instance.
         */
        protected EncodingTableBase()
        {
        }

        /**
         * Gets an item in the table by Unicode character.
         * 
         * @param unicode
         *            the Unicode character
         * 
         * @return the encoding table item
         */
        public EncodingTableItem this[char unicode]
        {
            get
            {
                return unicodeMap[unicode];
            }
        }

        /**
         * Gets an output character in the table by Arabic character type.
         * 
         * @param characterType
         *            the type of Arabic character
         * 
         * @return the output character in the table
         */
        public char getCharacter(CharacterType characterType)
        {
            return characterList[(int)characterType];
        }

        /**
         * Gets an output character in the table by Unicode character type.
         * 
         * @param unicodeType
         *            the type of Unicode character
         * 
         * @return the output character in the table
         */
        public char getCharacter(UnicodeType unicodeType)
        {
            return unicodeList[(int)unicodeType];
        }

        /**
         * Adds an Arabic character to the table.
         * 
         * @param unicodeType
         *            the type of Unicode character
         * 
         * @param ch
         *            the output character
         * 
         * @param characterType
         *            the type of Arabic character
         */
        protected void addItem(UnicodeType unicodeType, char ch, CharacterType characterType)
        {
            this.addItem(unicodeType, ch, characterType, DiacriticType.Unknown);
        }

        /**
         * Adds a diacritic to the table.
         * 
         * @param unicodeType
         *            the type of Unicode character
         * 
         * @param ch
         *            the output character
         * 
         * @param diacriticType
         *            the type of diacritic
         */
        protected void addItem(UnicodeType unicodeType, char ch, DiacriticType diacriticType)
        {
            this.addItem(unicodeType, ch, CharacterType.Unknown, diacriticType);
        }

        /**
         * Adds an Arabic character with an attached diacritic to the table.
         * 
         * @param unicodeType
         *            the type of Unicode character
         * 
         * @param ch
         *            the output character
         * 
         * @param characterType
         *            the type of Arabic character
         * 
         * @param diacriticType
         *            the type of diacritic
         */
        protected void addItem(UnicodeType unicodeType, char ch, CharacterType characterType, DiacriticType diacriticType)
        {

            // Create the item that will be added to the table.
            EncodingTableItem item = new EncodingTableItem(characterType, diacriticType);

            // Unicode --> item
            unicodeMap[ch] = item;

            // Character type --> Unicode
            if (characterType != CharacterType.Unknown && diacriticType == DiacriticType.Unknown)
            {
                characterList[(int)characterType] = ch;
            }

            // Unicode type --> Unicode
            unicodeList[(int)unicodeType] = ch;
        }
    }
}