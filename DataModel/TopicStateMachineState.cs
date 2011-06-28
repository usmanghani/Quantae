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

    }
}
