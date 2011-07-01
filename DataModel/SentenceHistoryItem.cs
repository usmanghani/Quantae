namespace Quantae.DataModel
{
    public class SentenceHistoryItem : HistoryItem
    {
        public SentenceHandle Sentence { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as SentenceHistoryItem).Sentence.Equals(this.Sentence);
        }

        public override int GetHashCode()
        {
            return this.Sentence.GetHashCode();
        }
    }
}