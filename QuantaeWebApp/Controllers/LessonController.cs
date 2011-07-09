using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quantae.Engine;
using Quantae.ViewModels;
using Quantae.DataModel;

namespace QuantaeWebApp.Controllers
{
    public class LessonController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            // PRE: The session is already created by the Account Controller. LogOn Action.
            // PRE: The token is in the cookie.
            // TODO: StartSessionAlgo
            // 1. Get the topic history for the user.
            // 2. Lesson Plan.
            // 3. Current Topic.
            // 4. Set Type of Slide to Lesson Hub. (Lesson Hub talks to the engine and does GetNextTopic.)
            // POST: Returns the Lesson Hub

            // TODO: Create Lesson Hub view here.

            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);
            //Topic currentTopic = TopicOperations.GetTopicFromHandle(profile.CurrentState.CourseLocationInfo.CurrentTopic.Topic);

            LessonHubModel model = new LessonHubModel();

            //model.CurrentTopicName = currentTopic.TopicName;
            model.CurrentTopicName = "Blah";
            model.TopicHistory = new List<string>();

            foreach (var thi in profile.History.TopicHistory)
            {
                model.TopicHistory.Add(TopicOperations.GetTopicFromHandle(thi.Topic).TopicName);
            }

            return View("LessonHub", model);
        }

        [Authorize]
        public ActionResult ContinueTopic()
        {
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult RestartTopic()
        {
            // PRE: Start Session.
            // PRE: Token in the cookie.
            // TODO: RestartTopicAlgo
            // 1. Get Current Topic.
            // 2. TopicOperations.RestartCurrentTopic. (modifies certain data structures to make it look like the topic is starting again.)
            // 3. We need to return the first intro slide of this topic.
            // POST: returns the first intro slide of this topic.

            // TODO: Redirect to action here.
            return RedirectToAction("Index", "Lesson");
        }

        [Authorize]
        public ActionResult SkipTopic()
        {
            // PRE: Start Session
            // PRE: Token in the cookie.
            // TODO: SkipTopicAlgo
            // 1. Mark topic skipped. (It still goes to your history, but it is considered successful.)
            // 2. Return Lesson Hub.
            // POST: Return Lesson Hub. (with updated info).

            // TODO: Redirect to action here.
            return RedirectToAction("About", "Home");
        }
    }
}
