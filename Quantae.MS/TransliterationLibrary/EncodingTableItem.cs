namespace Quantae.Transliteration.Encoding
{
    public class EncodingTableItem
    {

        public EncodingTableItem(CharacterType characterType, DiacriticType diacriticType)
        {
            this.CharacterType = characterType;
            this.DiacriticType = diacriticType;
        }

        public CharacterType CharacterType { get; set; }

        public DiacriticType DiacriticType { get; set; }
    }
}