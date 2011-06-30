namespace Quantae.DataModel
{
    public enum AnswerDimension
    {
        Unknown = 0,

        // A failure on this becomes Major weakness.
        Understanding = 1,

        // Minor weakness.
        NounConjugationNumber = 2,
        NounConjugationGender = 3,

        // FUTURE: We may or may not use this
        NounConjugationState = 4,

        // Minor weakness.
        VerbConjugationNumber = 5,
        VerbConjugationGender = 6,
        VerbConjugationTense = 8,
        VerbConjugationPerson = 9,

        // FUTURE: We may or may not use this
        VerbConjugationState = 7,

        // This is used to update your contextual/analytical score.
        GrammaticalAnalysis = 10,

        // This is used to update your vocab history.
        Vocabulary = 11,
    }
}