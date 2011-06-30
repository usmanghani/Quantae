using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quantae.DataModel
{
    public class TopicStateMachineState
    {
        public bool IsIntroComplete { get; set; }
        public int IntroSlideIndex { get; set; }
        public TopicSectionType CurrentSection { get; set; }

        public SampleSectionState SampleSectionState { get; set; }
        public int SampleSectionIterationCount { get; set; }

        public TopicStateMachineState()
        {
            SampleSectionState = new SampleSectionState();
            SampleSectionIterationCount = 0;

            IsIntroComplete = false;
            IntroSlideIndex = 0;

            CurrentSection = TopicSectionType.Intro;
        }
    }
}
