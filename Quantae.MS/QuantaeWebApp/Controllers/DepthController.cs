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
    public class DepthController : Controller
    {
        //
        // GET: /Depth/
        [Authorize]
        public ActionResult Index()
        {
            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);
            DepthSlideViewModel model = null;
            string content = GetNextDepthSlideContent(profile);

            // Empty content means the intro section is complete (TODO: change this to a proper data contract)
            if (string.IsNullOrEmpty(content))
            {
                return RedirectToAction("Next", "Section");
            }
            else
            {
                model = new DepthSlideViewModel(content);
            }

            return View(ViewNames.Depth.DepthSlideView, model);
        }

        private string GetNextDepthSlideContent(UserProfile userProfile)
        {
            Topic currentTopic = Repositories.Topics.GetItemByHandle(userProfile.CurrentState.CourseLocationInfo.CurrentTopic.Topic);

            if (userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection == TopicSectionType.Depth &&
               !userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IsDepthComplete &&
                userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.DepthSlideIndex < currentTopic.DepthSection.Pages.Count)
            {
                int idx = ++userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.DepthSlideIndex;

                // BUG: Separate this out.
                VocabOperations.UpdateVocabulary(userProfile, currentTopic.DepthSection.Pages[idx].VocabEntries, VocabRankTypes.CorrectOrSeenInIntro);
                return currentTopic.DepthSection.Pages[idx].Content;
            }

            userProfile.CurrentState.CourseLocationInfo.TopicLocationInfo.IsDepthComplete = true;

            return string.Empty;
        }
    }
}
