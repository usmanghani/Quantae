namespace Quantae.DataModel.Sql
{
    public class TopicLocationInfo : QuantaeObject
    {
        public bool IsIntroComplete { get; set; }
        public int IntroSlideIndex { get; set; }
        public TopicSectionType CurrentSection { get; set; }
        public SampleSectionState SampleSectionState { get; set; }
        public int SampleSectionIterationCount { get; set; }
        public int QuestionCount { get; set; }

        public TopicLocationInfo()
        {
            this.SampleSectionState = new SampleSectionState();
            this.SampleSectionIterationCount = 0;

            this.IsIntroComplete = false;
            this.IntroSlideIndex = 0;

            this.CurrentSection = TopicSectionType.Intro;
        }
    }
}