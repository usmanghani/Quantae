﻿using System.Text;
namespace Quantae.Transliteration.Encoding{

/**
 * <code>ArabicEncoderBase</code> is an abstract base class providing a common
 * implementation for {@link org.jqurantree.arabic.ArabicText} encoders. The
 * class supports the
 * {@link org.jqurantree.arabic.ArabicText#toString(EncodingType)} method by
 * implementing table-driven encoding. An {@link EncodingTableBase} instance is
 * used to lookup the mapping for each character in the source text.
 * <p>
 * The following encoding algorithm is reversible, ensuring that round trip
 * testing is possible. For each {@link org.jqurantree.arabic.ArabicCharacter}:
 * <p>
 * Step 1. If the letter or Quranic symbol has a diacritic that forms a well
 * known combination, then map this onto a single output character. If <i>Hamza
 * above</i> was the diacritic used, then remove this from the list of
 * diacritics to consider. The 6 well known combinations are:
 * <p>
 * <i>- Alif/Waw/Ya + Hamza above<br/> - Alif + Hamza below<br/> - Alif + Hamzat
 * wasl<br/> - Alif + Khanjareeya (superscript Alif)<br/></i>
 * <p>
 * Step 2. If Step 1 did not apply, then use the {@link EncodingTableBase}
 * instance to determine the output character to use for the letter or Quranic
 * symbol, without its diacritics.
 * <p>
 * Step 3. Use the encoding table to form output characters out any remaining
 * diacritics, in the following order:
 * <p>
 * <i>- Hamza above<br/> - Shadda<br/> - Fathatan<br/> - Dammatan<br/> -
 * Kasratan<br/> - Fatha<br/> - Damma<br/> - Kasra<br/> - Sukun<br/> -
 * Maddah<br/></i>
 * 
 * @author Kais Dukes
 */
public abstract class ArabicEncoderBase : ArabicEncoder {

    private byte[] buffer;
    private int offset;
    private byte value;
    private bool isMaddah;
    private bool isHamzaAbove;
    private EncodingTableBase encodingTable;
    private EncodingOptions options;
 
    /**
     * A <code>string</code> buffer used to hold the encoder's plain text
     * output.
     */
    protected StringBuilder text = new StringBuilder();

    /**
     * Creates a new encoder.
     */
    protected ArabicEncoderBase() {
        encodingTable = null;
    }

    /**
     * Creates a new encoder using the specified encoding table.
     * 
     * @param encodingTable
     *            the encoding table to use when performing table-driven
     *            encoding.
     */
    protected ArabicEncoderBase(EncodingTableBase encodingTable) {
        this.encodingTable = encodingTable;
    }

    public string Encode(byte[] buffer, int offset, int characterCount, EncodingOptions options) {

        // Initiate.
        this.options = options;

        // Encode each Arabic character.
        for (int i = 0; i < characterCount; i++) {

            // Seperator.
            if (i > 0) {
                writeCharacterSeperator();
            }

            // Encode character.
            encodeCharacter(buffer, offset);
            offset += ByteFormat.CHARACTER_WIDTH;
        }

        // Return text.
        return text.ToString();
    }

    /**
     * Overriden by derived encoders to write a seperator between each
     * {@link org.jqurantree.arabic.ArabicCharacter}.
     * 
     */
    protected void writeCharacterSeperator() {
    }

    /**
     * Encodes a single {@link org.jqurantree.arabic.ArabicCharacter} in the
     * internal {@link org.jqurantree.arabic.ByteFormat}.
     * 
     * @param buffer
     *            the <code>byte[]</code> buffer holding the character
     * @param offset
     *            the offset of the character within the buffer
     */
    protected void encodeCharacter(byte[] buffer, int offset) {

        // Initiate.
        this.buffer = buffer;
        this.offset = offset;

        // Whitespace?
        value = buffer[offset];
        if (value == ByteFormat.WHITESPACE) {

            // Write whitespace.
            text.Append(' ');

        } else {

            // Write character.
            text.Append(getCharacter());

            // Write diacritics.
            writeDiacritics();
        }
    }

    private char getCharacter() {

        // Initiate.
        CharacterType characterType = (CharacterType)value;
        UnicodeType unicodeType = UnicodeType.Unknown;
        isMaddah = ByteFormat.isMaddah(buffer, offset);
        isHamzaAbove = ByteFormat.isHamzaAbove(buffer, offset);

        // Alif + Maddah
        if (options == EncodingOptions.CombineAlifWithMaddah
                && characterType == CharacterType.Alif
                && ByteFormat.isMaddah(buffer, offset)) {
            unicodeType = UnicodeType.AlifWithMaddah;
            isMaddah = false;
        }

        // Alif + Hamza above
        else if (characterType == CharacterType.Alif && isHamzaAbove
                && !ByteFormat.isAlifKhanjareeya(buffer, offset)) {
            unicodeType = UnicodeType.AlifWithHamzaAbove;
            isHamzaAbove = false;
        }

        // Waw + Hamza above
        else if (characterType == CharacterType.Waw && isHamzaAbove) {
            unicodeType = UnicodeType.WawWithHamzaAbove;
            isHamzaAbove = false;
        }

        // Alif + Hamza below
        else if (characterType == CharacterType.Alif
                && ByteFormat.isHamzaBelow(buffer, offset)) {
            unicodeType = UnicodeType.AlifWithHamzaBelow;
        }

        // Ya + Hamza above
        else if (characterType == CharacterType.Ya && isHamzaAbove) {
            unicodeType = UnicodeType.YaWithHamzaAbove;
            isHamzaAbove = false;
        }

        // Alif Khanjareeya
        else if (characterType == CharacterType.Alif
                && ByteFormat.isAlifKhanjareeya(buffer, offset)) {
            unicodeType = UnicodeType.AlifKhanjareeya;
        }

        // Alif + Hamzat Wasl
        else if (characterType == CharacterType.Alif
                && ByteFormat.isHamzatWasl(buffer, offset)) {
            unicodeType = UnicodeType.AlifWithHamzatWasl;
        }

        // Return character.
        return unicodeType != null ? encodingTable.getCharacter(unicodeType)
                : encodingTable.getCharacter(characterType);
    }

    private void writeDiacritics() {

        // Hamza above.
        if (isHamzaAbove) {
            text.Append(encodingTable.getCharacter(UnicodeType.HamzaAbove));
        }

        // Shadda.
        if (ByteFormat.isShadda(buffer, offset)) {
            text.Append(encodingTable.getCharacter(UnicodeType.Shadda));
        }

        // Fathatan.
        if (ByteFormat.isFathatan(buffer, offset)) {
            text.Append(encodingTable.getCharacter(UnicodeType.Fathatan));
        }

        // Dammatan.
        if (ByteFormat.isDammatan(buffer, offset)) {
            text.Append(encodingTable.getCharacter(UnicodeType.Dammatan));
        }

        // Kasratan.
        if (ByteFormat.isKasratan(buffer, offset)) {
            text.Append(encodingTable.getCharacter(UnicodeType.Kasratan));
        }

        // Fatha.
        if (ByteFormat.isFatha(buffer, offset)) {
            text.Append(encodingTable.getCharacter(UnicodeType.Fatha));
        }

        // Damma.
        if (ByteFormat.isDamma(buffer, offset)) {
            text.Append(encodingTable.getCharacter(UnicodeType.Damma));
        }

        // Kasra.
        if (ByteFormat.isKasra(buffer, offset)) {
            text.Append(encodingTable.getCharacter(UnicodeType.Kasra));
        }

        // Sukun.
        if (ByteFormat.isSukun(buffer, offset)) {
            text.Append(encodingTable.getCharacter(UnicodeType.Sukun));
        }

        // Maddah.
        if (isMaddah) {
            text.Append(encodingTable.getCharacter(UnicodeType.Maddah));
        }
    }
}
}