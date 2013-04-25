using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quantae.DataModel;

namespace Quantae.Engine
{
    public class SectionUtilities
    {
        public static bool StartingExercisePack(UserProfile profile)
        {
            var topicLocationInfo = profile.CurrentState.CourseLocationInfo.TopicLocationInfo;
            bool result = topicLocationInfo.CurrentSection == TopicSectionType.Exercise;
            result = topicLocationInfo.IsIntroComplete == true;
            result = topicLocationInfo.ExerciseSectionState.IsQuestion == false;
            return result;
        }
    }
}
