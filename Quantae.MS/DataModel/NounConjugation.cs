namespace Quantae.DataModel
{
    public class NounConjugation : Conjugation
    {
        public override bool Equals(object obj)
        {
            NounConjugation item = obj as NounConjugation;

            return item.Gender == this.Gender && item.Number == this.Number;
        }

        public override int GetHashCode()
        {
            return string.Format("{0}{1}", this.Gender.ToString(), this.Number.ToString()).GetHashCode();
        }
    }
}