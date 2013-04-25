using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class GetNextSentenceResult
    {
        public bool Success { get; private set; }
        public Sentence Sentence { get; private set; }
        public bool IsQuestion { get; private set; }
        public QuestionDimension QuestionDimension { get; private set; }
        public bool IsReview { get; private set; }

        public GetNextSentenceResult(Sentence sentence, bool success = true, bool isQuestion = false, QuestionDimension dimension = QuestionDimension.Unknown, bool isReview = false)
        {
            this.Sentence = sentence;
            this.Success = success;
            this.IsQuestion = isQuestion;
            this.IsReview = isReview;
            this.QuestionDimension = dimension;
        }
    }
}
