namespace Quantae.DataModel
{
    public class VocabularyHistoryItem : HistoryItem
    {
        public VocabEntryHandle VocabEntry { get; set; }

        /// <summary>
        ///  Vocabulary Rank:
        ///  1. 0 for unknown word.
        ///  2. +1 for Wrong
        ///  3. +2  when encountered in sample/question
        ///  4. +3 Correct or shown in intro.
        /// </summary>
        public VocabRankTypes Rank { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as VocabularyHistoryItem).VocabEntry.Equals(this.VocabEntry);
        }

        public override int GetHashCode()
        {
            return this.VocabEntry.GetHashCode();
        }
    }
}