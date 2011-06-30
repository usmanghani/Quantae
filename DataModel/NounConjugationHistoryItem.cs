namespace Quantae.DataModel
{
    public class NounConjugationHistoryItem : HistoryItem
    {
        public NounConjugation NounConjugation { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as NounConjugationHistoryItem).NounConjugation.Equals(this.NounConjugation);
        }

        public override int GetHashCode()
        {
            return this.NounConjugation.GetHashCode();
        }
    }
}