﻿using System.Collections.Generic;

namespace Quantae.DataModel
{
    public class Question
    {
        public string QuestionString { get; set; }
        public string QuestionSubstring { get; set; }

        public QuestionDimension Dimension { get; set; }

        public List<AnswerChoice> AnswerChoices { get; set; }
        public List<string> AnswerSegments { get; set; }
        public List<int> BlankPositions { get; set; }
        public int CorrectAnswerIndex { get; set; }

        /// <summary>
        /// This is exclusively used to identify the umbrella topic being covered in revision questions.
        /// </summary>
        public string RevisionTopicTag { get; set; }

        public Question()
        {
            this.AnswerChoices = new List<AnswerChoice>();
        }
    }
}