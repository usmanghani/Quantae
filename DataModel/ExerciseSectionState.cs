namespace Quantae.DataModel
{
    public class ExerciseSectionState
    {
        public SentenceHandle CurrentSentence { get; set; }
        public bool IsQuestion { get; set; }
        public QuestionDimension CurrentQuestionDimension { get; set; }
    }
}