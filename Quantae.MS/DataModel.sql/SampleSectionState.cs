namespace Quantae.DataModel.Sql
{
    public class SampleSectionState : QuantaeObject
    {
        public SentenceHandle CurrentSentence { get; set; }
        public bool IsQuestion { get; set; }
        public QuestionDimension CurrentQuestionDimension { get; set; }
    }
}