using System.Collections.Generic;

namespace Quantae.DataModel
{
    public class Question
    {
        public string QuestionString { get; set; }
        public string QuestionSubstring { get; set; }

        public List<AnswerChoice> AnswerChoices { get; set; }
        public QuestionDimension Dimension { get; set; }

        /// <summary>
        /// This is exclusively used to identify the umbrella topic being covered in revision questions.
        /// </summary>
        public string RevisionTopicTag { get; set; }
        public List<string> AnswerSegments { get; set; }
        public int BlankPosition { get; set; }
    }
}