using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class SampleSectionState
    {
        public SentenceHandle CurrentSentence { get; set; }
        public bool IsQuestion { get; set; }
        public QuestionDimension CurrentQuestionDimension { get; set; }
    }
}
