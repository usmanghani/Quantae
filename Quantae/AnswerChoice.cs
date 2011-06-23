using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class AnswerChoice
    {
        public string Answer { get; set; }
        public GrammarRoleHandle GrammarRole { get; set; }
        public AnswerDimension Dimension { get; set; }
        public bool IsCorrect { get; set; }
    }
}
