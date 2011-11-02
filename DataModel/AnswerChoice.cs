namespace Quantae.DataModel
{
    public class AnswerChoice
    {
        public string Answer { get; set; }

        // this is for hints.
        public GrammarRoleHandle GrammarRole { get; set; }

        // this is for hints.
        public AnswerDimension Dimension { get; set; }
    }
}