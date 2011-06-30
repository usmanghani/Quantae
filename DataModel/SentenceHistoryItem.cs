namespace Quantae.DataModel
{
    public class SentenceHistoryItem : HistoryItem
    {
        public SentenceHandle Sentence { get; set; }
        public SentenceFragment Intent { get; set; }

        // only used if the Intent was to show the question.
        public QuestionDimension QuestionDimension { get; set; }

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