using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quantae.DataModel;
using Quantae.ViewModels;

namespace QuantaeWebApp
{
    public interface ISlideSelectionEngine
    {
        SlideView GetNextSlide(UserProfile profile);
    }

    public class SlideSelectionEngine : ISlideSelectionEngine
    {
        public SlideView GetNextSlide(UserProfile profile)
        {
            if (profile.CurrentState.CourseLocationInfo.CourseLocation == CourseLocation.Topic)
            {
                //if (sectionType == TopicSectionType.Intro)
                //{
                //    //  2.a.if Intro Section then GetNextIntroPage
                //    //      2.a.i Set Slide Type to Intro
                //    //      2.a.ii. Return slide.
                //}
                //else if (sectionType == TopicSectionType.Exercise)
                //{
                //    //  2.b.else if Current Section is Exercise then GetNextSentence
                //    //      2.b.i Set Slide Type to Question or Sample Sentence accordingly.
                //    //      2.b.ii Return slide.

                //}
                //else if (sectionType == TopicSectionType.Review)
                //{
                //    //  2.b.else if Current Section is Review then GetNextSentence
                //    //      2.b.i Set Slide Type to Question or Sample Sentence accordingly.
                //    //      2.b.ii Return slide.
                //}
                //else if (sectionType == TopicSectionType.Extras)
                //{
                //    //  2.c. If the Current Section is Extras, then GetExtrasHubContent
                //    //      2.c.i. Set Slide Type to Extras Hub.
                //    //      2.c.ii. Return slide.
                //}
                //else if (sectionType == TopicSectionType.Depth)
                //{
                //    //  2.d. If Current section is Depth then GetNextDepthPage
                //    //      2.d.i. Set Slide Type to Dept.
                //    //      2.d.ii. Return slide.
                //}
                //else
                //{
                //    return null;
                //}

            }
            else if (profile.CurrentState.CourseLocationInfo.CourseLocation == CourseLocation.LessonHub)
            {
                // TODO: Return Lesson Hub slide here.
            }

            throw new NotImplementedException();
        }
    }
}