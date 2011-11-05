using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quantae.DataModel;
using Quantae.Repositories;
using Quantae.Engine;
using Quantae.ViewModels;

namespace QuantaeWebApp.Controllers
{
    public class IntroController : Controller
    {
        //
        // GET: /Intro/
        [Authorize]
        public ActionResult Index()
        {
            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);
            IntroSlideViewModel model = null;
            string content = GetNextIntroSlideContent(profile);

            // Empty content means the intro section is complete (TODO: change this to a proper data contract)
            if (string.IsNullOrEmpty(content))
            {
                return RedirectToAction("Next", "Section");
            }
            else
            {
                model = new IntroSlideViewModel(content);
            }

            return View(ViewNames.Intro.IntroSlideView, model);
        }

        private static string GetNextIntroSlideContent(UserProfile userProfile)
        {
            string currentIntroSlideContent = string.Empty;

            Topic currentTopic = Repositories.Topics.GetItemByHandle(userProfile.CurrentState.CourseLocationInfo.CurrentTopic.Topic);

            if (currentTopic.IntroSection.Pages.Count > 0 &&
                userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection == TopicSectionType.Intro &&
               !userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IsIntroComplete &&
                userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IntroSlideIndex < currentTopic.IntroSection.Pages.Count)
            {
                int idx = ++userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IntroSlideIndex;

                // BUG: Separate this out.
                VocabOperations.UpdateVocabulary(userProfile, currentTopic.IntroSection.Pages[idx].VocabEntries, VocabRankTypes.CorrectOrSeenInIntro);
                currentIntroSlideContent = currentTopic.IntroSection.Pages[idx].Content;
            }
            else
            { 
                userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IsIntroComplete = true; 
            }

            return currentIntroSlideContent;
        }
    }
}
