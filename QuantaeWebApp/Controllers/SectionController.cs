using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quantae.DataModel;
using Quantae.Repositories;
using Quantae.Engine;
using System.Diagnostics;

namespace QuantaeWebApp.Controllers
{
    public class SectionController : Controller
    {
        //
        // GET: /Section/

        public ActionResult Index()
        {
            RedirectToRouteResult result = null;
            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);
            var currentTopic = profile.CurrentState.CourseLocationInfo.CurrentTopic;
            var currentSection = profile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection;

            switch (currentSection)
            {
                case TopicSectionType.Unknown:
                    if (currentTopic.IsPseudoTopic)
                    {
                        profile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection = TopicSectionType.Exercise;
                        result = RedirectToAction("Index", "Exercise");
                    }
                    else
                    {
                        profile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection = TopicSectionType.Intro;
                        result = RedirectToAction("Index", "Intro");
                    }

                    break;
                case TopicSectionType.Intro:
                    if (currentTopic.IsPseudoTopic)
                    {
                        profile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection = TopicSectionType.Exercise;
                        result = RedirectToAction("Index", "Exercise");
                    }
                    else
                    {
                        result = RedirectToAction("Index", "Intro");
                    }

                    break;
                case TopicSectionType.Exercise:
                    result = RedirectToAction("Index", "Exercise");
                    break;
                case TopicSectionType.Review:
                    result = RedirectToAction("Index", "Review");
                    break;
                case TopicSectionType.Extras:
                    result = RedirectToAction("Index", "Extras");
                    break;
                case TopicSectionType.Depth:
                    result = RedirectToAction("Index", "Depth");
                    break;
            }

            Repositories.Users.Save(profile);
            return result;
        }

        /// <summary>
        /// Once any sub controller (exercise, intro, depth, review) is done it is responsible for redirecting here.
        /// /Section/Next
        /// </summary>
        /// <returns></returns>
        public ActionResult Next()
        {
            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);
            var currentTopic = profile.CurrentState.CourseLocationInfo.CurrentTopic;
            var currentSection = profile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection;
            string nextActionName = "Index";
            TopicSectionType nextSection = TopicSectionType.Unknown;

            switch (currentSection)
            {
                case TopicSectionType.Unknown:
                    Trace.TraceError("Unexpected Next action received in SectionController.");
                    break;
                case TopicSectionType.Intro:
                    // Intro + Next = Excercise
                    if (currentTopic.IsPseudoTopic)
                    {
                        Trace.TraceError("Unexpected Next action received in SectionController. Pseudo topic should not be in intro.");
                    }
                    else
                    {
                        nextSection = TopicSectionType.Exercise;                     
                    }

                    break;
                case TopicSectionType.Exercise:
                    // Exercise + Next = Review
                    nextSection = TopicSectionType.Review;
                    break;
                case TopicSectionType.Review:
                    // Review + Next  = Extras
                    nextSection = TopicSectionType.Extras;
                    break;
                case TopicSectionType.Extras:
                    // Extras + Next = Depth
                    nextSection = TopicSectionType.Depth;
                    break;
                case TopicSectionType.Depth:
                    // Depth + Next = Next of Lesson; go up (like Salmons)
                    nextActionName = "Next";
                    break;
            }

            string nextSectionName;
            if (nextSection == TopicSectionType.Unknown)
            {
                nextSectionName = "Lesson";
            }
            else
            {
                nextSectionName = nextSection.ToString();
                profile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection = nextSection;
            }
            
            Repositories.Users.Save(profile);
            return RedirectToAction(nextActionName, nextSectionName);
        }
    }
}

