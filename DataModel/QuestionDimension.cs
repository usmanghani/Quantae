namespace Quantae.DataModel
{
    public enum QuestionDimension
    {
        Unknown = 0,

        // updates vocab history.
        // English to Arabic. 
        // E.g.
        // What does this mean?
        // هَذَا كِتَابٌ
        // This is a book
        // This is a boy
        // This is a shelf
        // This is a nut
        VocabularyTargetToSource = 1,

        // updates vocab history.
        // Arabic to English.
        // E.g.
        // What is this in Arabic?
        // This is a book
        // هَذَا كِتَابٌ
        // هَذَا كَأْسٌ
        // هَذَا هَاتِفٌ
        // هَذَا مُعَلِّمٌ
        VocabularySourceToTarget = 6,

        // detects major weakness.
        Understanding = 2,

        // detects learning type.
        Grammar = 3,

        NounConjugation = 4,
        VerbConjugation = 5,
    }
}