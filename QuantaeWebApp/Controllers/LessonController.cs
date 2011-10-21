using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quantae.Engine;
using Quantae.ViewModels;
using Quantae.DataModel;
using Quantae.Repositories;
using System.Diagnostics;

namespace QuantaeWebApp.Controllers
{
    /// <summary>
    /// !!!!NOTE: Topic == Lesson!!!!
    /// </summary>
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

            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);
            TopicHistoryItem currentTopicHistoryItem = profile.CurrentState.CourseLocationInfo.CurrentTopic;
            ITopicGraphNavigator nav = new TopicGraphNavigator(Repositories.Topics);
            LessonHubViewModel model = null;
            GetNextTopicResult result = null;

            // TODO: un-comment. This was commented out to test the view.
            // We have completed the previous topic.
            // OR we are starting anew.
            if (currentTopicHistoryItem == null)
            {
                result = nav.GetNextTopic(profile);
                if (result.Success)
                {
                    model = new LessonHubViewModel(result.TargetTopic.TopicName, true);
                    UserOperations.UpdateUserCurrentTopic(profile, result);
                    Repositories.Users.Save(profile);
                }
                else
                {
                    Trace.TraceError("LessonController/Index: GetNextTopic returned failure.");
                    // TODO: Figure out what to do if we can't take the user to the nxt topic. message it properly here and ask the user to continue onto the same topic.
                }
            }
            else
            {
                Topic topicOperationsGetTopicFromHandle = TopicOperations.GetTopicFromHandle(currentTopicHistoryItem.Topic);
                if (topicOperationsGetTopicFromHandle == null)
                {
                    // TODO: Invalid handle. Figure out what to do here. Atleast trace.
                    Trace.WriteLine("LessonController/Index: Invalid topic handle. Topic handle doesn't have a corresponding topic entry.");
                }

                model = new LessonHubViewModel(topicOperationsGetTopicFromHandle.TopicName, false);
            }

            foreach (var thi in profile.History.TopicHistory)
            {
                model.AddHistory(thi);
            }

            return View(ViewNames.Lesson.LessonHubView, model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ContinueTopic(LessonHubViewModel viewModel)
        {
            // TODO: Redirect to correct action here.
            return RedirectToAction("Continue", "Section");
        }

        [Authorize]
        [HttpPost]
        public ActionResult RestartTopic(LessonHubViewModel viewModel)
        {
            // PRE: Start Session.
            // PRE: Token in the cookie.
            // TODO: RestartTopicAlgo
            // 1. Get Current Topic.
            // 2. TopicOperations.RestartCurrentTopic. 
            // (modifies certain data structures to make it look like the topic is starting again.)
            // 3. We need to return the first intro slide of this topic.
            // POST: returns the first intro slide of this topic.

            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);
            TopicOperations.RestartTopic(profile);
            Repositories.Users.Save(profile);
            // TODO: Redirect to correct action here.
            return RedirectToAction("Index", "Lesson");
        }

        [Authorize]
        [HttpPost]
        public ActionResult SkipTopic(LessonHubViewModel viewModel)
        {
            // PRE: Start Session
            // PRE: Token in the cookie.
            // TODO: SkipTopicAlgo
            // 1. Mark topic skipped. (It still goes to your history, but it is considered complete and successful.)
            // 2. Return Lesson Hub.
            // POST: Return Lesson Hub. (with updated info).

            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);

            // NOTE: This is intentional and by policy of the first founding father. SyedB.
            // It still goes to your history, but it is considered complete and successful. 
            // SkipTopic is an explicit request by the student that he/she doesn't want to see
            // this topic. Hence we mark it successful to prevent it from showing up again and again.
            TopicOperations.MarkCurrentTopicComplete(profile, true);
            
            Repositories.Users.Save(profile);

            return RedirectToAction("Index", "Lesson");
        }
    }
}
