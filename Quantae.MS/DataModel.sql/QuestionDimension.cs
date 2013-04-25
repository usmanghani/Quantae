namespace Quantae.DataModel.Sql
{
    public enum QuestionDimension
    {
        Unknown = 0,

        // updates vocab history.
        Vocab = 1,

        // detects major weakness.
        Understanding = 2,

        // detects learning type.
        Grammar = 3,

        NounConjugation = 4,
        VerbConjugation = 5,
    }
}