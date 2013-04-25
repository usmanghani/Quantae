namespace Quantae.DataModel.Sql
{
    [MongoDB.Bson.Serialization.Attributes.BsonKnownTypes(typeof(VerbConjugation), typeof(NounConjugation))]
    public abstract class Conjugation
    {
        public GenderRule Gender { get; set; }
        public NumberRule Number { get; set; }
    }
}