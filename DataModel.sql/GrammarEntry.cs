namespace Quantae.DataModel.Sql
{
    public class GrammarEntry : QuantaeObject
    {
        public string Text { get; set; }
        public string Translation { get; set; }
        public WordType WordType { get; set; }
        public Conjugation Conjugation { get; set; }
        public DefinitenessRule DefinitenessRule { get; set; }

        public override bool Equals(object obj)
        {
            return (obj as GrammarEntry).GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            return this.Text.GetHashCode() + this.Translation.GetHashCode();
        }
    }
}