
namespace Quantae.Transliteration.Encoding {


/**
 * <code>ArabicDecoderBase</code> is an abstract base class providing a common
 * implementation for {@link org.jqurantree.arabic.ArabicText} decoders. The
 * class supports the
 * {@link org.jqurantree.arabic.ArabicText#fromEncoding(String, EncodingType)}
 * method by implementing table-driven decoding. An {@link EncodingTableBase}
 * instance is used to lookup the mapping for each character in the source text.
 * 
 * @author Kais Dukes
 */
public abstract class ArabicDecoderBase : ArabicDecoder {

	private EncodingTableBase encodingTable;
	private ArabicTextBuilder builder = new ArabicTextBuilder();

	/**
	 * Creates a new decoder using the specified encoding table.
	 * 
	 * @param encodingTable
	 *            the encoding table to use when performing table-driven
	 *            decoding.
	 */
	protected ArabicDecoderBase(EncodingTableBase encodingTable) {
		this.encodingTable = encodingTable;
	}

	public byte[] Decode(string text) {

		// Decode each Unicode character.
		int size = text.Length;
		for (int i = 0; i < size; i++) {
			Decode(text[i]);
		}

		// Return the buffer.
		return builder.toByteArray();
	}

	private void Decode(char ch) {

		// Look up character type and diacritic type.
		EncodingTableItem item = encodingTable[ch];
		if (item != null) {
			CharacterType characterType = item.CharacterType;
			DiacriticType diacriticType = item.DiacriticType;

			// Add character.
			if (characterType != null) {
				builder.add(characterType);
			}

			// Add diacritic.
			if (diacriticType != null) {
				builder.add(diacriticType);
			}

		} else {

			// Treat any unknown characters as whitespace.
			builder.addWhitespace();
		}
	}
}
}