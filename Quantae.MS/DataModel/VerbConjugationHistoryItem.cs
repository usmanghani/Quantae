namespace Quantae.DataModel
{
    public class VerbConjugationHistoryItem : HistoryItem
    {
        public VerbConjugation VerbConjugation { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as VerbConjugationHistoryItem).VerbConjugation.Equals(this.VerbConjugation);
        }

        public override int GetHashCode()
        {
            return this.VerbConjugation.GetHashCode();
        }
    }
}