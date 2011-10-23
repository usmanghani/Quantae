using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuantaeWebApp
{
    // These are the names of the views we currently have.
    // They are grouped by controller names.
    // each view corresponds to a .cshtml file.
    public class ViewNames
    {
        public class Lesson
        {
            public const string LessonHubView = "LessonHub";
        }

        public class Section
        {
            public const string ExtrasHubView = "Extras";
        }

        public class Intro
        {
            public const string IntroSlideView = "IntroSlide";
        }

        public class Depth
        {
            public const string DepthSlideView = "DepthSlide";
        }

    }
}