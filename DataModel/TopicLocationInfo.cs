namespace Quantae.DataModel
{
    public class TopicLocationInfo
    {
        public bool IsIntroComplete { get; set; }
        public int IntroSlideIndex { get; set; }
        public TopicSectionType CurrentSection { get; set; }
        public ExerciseSectionState ExerciseSectionState { get; set; }
        public int SampleSectionIterationCount { get; set; }
        public int QuestionCount { get; set; }

        public TopicLocationInfo()
        {
            this.ExerciseSectionState = new ExerciseSectionState();
            this.SampleSectionIterationCount = 0;

            this.IsIntroComplete = false;
            this.IntroSlideIndex = 0;

            this.CurrentSection = TopicSectionType.Intro;
        }
    }
}