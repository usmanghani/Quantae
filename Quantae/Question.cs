using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae
{
    public class Question
    {
        public string QuestionString { get; set; }
        public string QuestionSubstring { get; set; }

        public List<AnswerChoice> AnswerChoices { get; set; }
        public QuestionDimension Dimension { get; set; }
        public List<string> AnswerSegments { get; set; }
        public int BlankPosition { get; set; }
    }
}
