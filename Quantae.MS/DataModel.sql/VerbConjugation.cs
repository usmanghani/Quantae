namespace Quantae.DataModel.Sql
{
    public class VerbConjugation : Conjugation
    {
        public PersonRule Person { get; set; }
        public TenseRule Tense { get; set; }

        public override bool Equals(object obj)
        {
            VerbConjugation item = obj as VerbConjugation;

            return (item.Gender == this.Gender)
                   && (item.Number == this.Number)
                   && (item.Person == this.Person)
                   && (item.Tense == this.Tense);
        }

        public override int GetHashCode()
        {
            return string.Format("{0}{1}{2}{3}", this.Gender.ToString(), this.Number.ToString(), this.Person.ToString(), this.Tense.ToString()).GetHashCode();
        }
    }
}