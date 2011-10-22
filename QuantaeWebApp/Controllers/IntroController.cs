using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quantae.DataModel;
using Quantae.Repositories;
using Quantae.Engine;

namespace QuantaeWebApp.Controllers
{
    public class IntroController : Controller
    {
        //
        // GET: /Intro/

        public ActionResult Index()
        {
            return View();
        }

        // TODO: This goes either into the controller or stays in a util class and gets called from the controller.
        public static string GetNextIntroSlideContent(UserProfile userProfile)
        {
            Topic currentTopic = Repositories.Topics.GetItemByHandle(userProfile.CurrentState.CourseLocationInfo.CurrentTopic.Topic);

            if (userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection == TopicSectionType.Intro &&
               !userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IsIntroComplete &&
                userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IntroSlideIndex < currentTopic.IntroSection.Pages.Count)
            {
                int idx = ++userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IntroSlideIndex;

                // BUG: Separate this out.
                VocabOperations.UpdateVocabulary(userProfile, currentTopic.IntroSection.Pages[idx].VocabEntries, VocabRankTypes.CorrectOrSeenInIntro);
                return currentTopic.IntroSection.Pages[idx].Content;
            }

            userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IsIntroComplete = true;
            userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection = TopicSectionType.Exercise;

            return string.Empty;
        }

    }
}
