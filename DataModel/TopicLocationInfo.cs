using System.Collections.Generic;
namespace Quantae.DataModel
{
    /// <summary>
    /// 
    /// </summary>
    public class TopicLocationInfo
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is intro complete.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is intro complete; otherwise, <c>false</c>.
        /// </value>
        public bool IsIntroComplete { get; set; }

        /// <summary>
        /// Gets or sets the index of the intro slide.
        /// </summary>
        /// <value>
        /// The index of the intro slide.
        /// </value>
        public int IntroSlideIndex { get; set; }

        /// <summary>
        /// Gets or sets the current section.
        /// </summary>
        /// <value>
        /// The current section.
        /// </value>
        public TopicSectionType CurrentSection { get; set; }

        /// <summary>
        /// Gets or sets the state of the exercise section.
        /// </summary>
        /// <value>
        /// The state of the exercise section.
        /// </value>
        public ExerciseSectionState ExerciseSectionState { get; set; }

        /// <summary>
        /// Gets or sets the exercise section iteration count.
        /// This counts how many times you have seen this sequence:
        /// (Sentence -> QD1 -> QD2 -> QD3 -> QD4 -> QD5){1, N}
        /// </summary>
        /// <value>
        /// The exercise section iteration count.
        /// </value>
        public int ExerciseSectionIterationCount { get; set; }

        /// <summary>
        /// Gets or sets the question count.
        /// </summary>
        /// <value>
        /// The question count.
        /// </value>
        public Dictionary<QuestionDimension, int> QuestionCountByQuestionDimension { get; set; }

        public TopicLocationInfo()
        {
            Reset();
        }

        public void Reset()
        {
            this.ExerciseSectionState = new ExerciseSectionState();
            this.ExerciseSectionIterationCount = 0;
            this.QuestionCountByQuestionDimension = new Dictionary<QuestionDimension, int>();

            this.IsIntroComplete = false;
            this.IntroSlideIndex = 0;

            this.CurrentSection = TopicSectionType.Intro;
        }
    }
}