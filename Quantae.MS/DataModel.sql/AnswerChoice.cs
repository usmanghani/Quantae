﻿namespace Quantae.DataModel.Sql
{
    public class AnswerChoice
    {
        public string Answer { get; set; }
        public GrammarRoleHandle GrammarRole { get; set; }
        public AnswerDimension Dimension { get; set; }
        public bool IsCorrect { get; set; }
    }
}