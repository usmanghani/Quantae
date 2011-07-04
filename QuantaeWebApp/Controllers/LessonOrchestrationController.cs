using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quantae.Engine;

namespace QuantaeWebApp.Controllers
{
    public class LessonOrchestrationController : Controller
    {
        //
        // GET: /LessonOrchestration/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StartSession()
        {
            // PRE: The session is already created by the Account Controller. LogOn Action.
            // PRE: The token is in the cookie.
            // TODO: 
            // 1. Get the topic history for the user.
            // 2. Lesson Plan.
            // 3. Current Topic.
            // 4. Set Type of Slide to Lesson Hub. (Lesson Hub talks to the engine and does GetNextTopic.)
            // POST: Returns the Lesson Hub

            return Json(null);
        }

        public ActionResult RestartTopic()
        {
            // PRE: Start Session.
            // PRE: Token in the cookie.
            // TODO:
            // 1. Get Current Topic.
            // 2. TopicOperations.RestartCurrentTopic. (modifies certain data structures to make it look like the topic is starting again.)
            // 3. We need to return the first intro slide of this topic.
            // POST: returns the first intro slide of this topic.

            return Json(null);
        }

        public ActionResult SkipTopic()
        {
            // PRE: Start Session
            // PRE: Token in the cookie.
            // TODO:
            // 1. Mark topic skipped. (It still goes to your history, but it is considered successful.)
            // 2. Return Lesson Hub.
            // POST: Return Lesson Hub. (with updated info).

            return Json(null);
        }

        /// <summary>
        /// Gets the next slide.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetNextSlide()
        {
            /*
                public enum TopicSectionType
                {
                    Unknown = 0,
                    Intro = 1,
                    Exercise = 2,
                    Revision = 3,
                    Extras = 4,
                    Depth = 5,
                }
             */

            // PRE: StartSession
            // PRE: Token in the cookie.
            // 1. Submit result from previous slide. SubmitResponse -> Flip to next section.
            // 2. Little Decision tree (Thomas)
            //  2a. Get Current Section
            //  2.a.if Intro Section then GetNextIntroSlide
            //      2.a.i Set Slide Type to Intro
            //      2.a.ii. Return slide.
            //  2.b.else if Current Section is Exercise then GetNextSentence
            //      2.b.i Set Slide Type to Question or Sample Sentence accordingly.
            //      2.b.ii Return slide.
            //  2.c.else if Current Section is Review then GetNextSentence
            //      2.c.i Set Slide Type to Question or Sample Sentence accordingly.
            //      2.c.ii Return slide.
            //  2.d. If the Current Section is Extras, then GetExtrasHubContent
            //      2.d.i. Set Slide Type to Extras Hub.
            //      2.d.ii. Return slide.
            //  2.e. If Current section is Depth then GetNextDepthSlide
            //      2.e.i. Set Slide Type to Dept.
            //      2.e.ii. Return slide.
            //  2.f. If Current section type is Completed
            //      2.f.i. Goto Lesson Hub.
            // POST: Returns intro, depth, extras hub, sentence, question slides or lesson hub.

            return Json(null);
        }
    }
}
