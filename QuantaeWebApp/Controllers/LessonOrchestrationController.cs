﻿using System;
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
            // PRE: StartSession
            // PRE: Token in the cookie.
            // 1. Submit result from previous slide.
            //  1.a. Update User State
            //  1.b. Evaluate User Competency
            //  1.c. EvaluateNextState -> Flip to next section, or topic.
            // 2. Little Decision tree (Thomas)
            // if Course Location is topic 
            //  2.a. Get Current Section
            //  2.a. if Intro Section then GetNextIntroSlide
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
            // Else 
            //  2.f. If Current section type is Completed
            //      2.f.i. Goto Lesson Hub.
            // POST: Returns intro, depth, extras hub, sentence, question slides or lesson hub.

            // Update user state:
            // 1. Update vocab, noun, verb, sentence histories if sample sentence.
            // 2. Update current state with Answer failure and success counts if question.
            // 3. Update Learning Scores.

            // Evaluate user competency:
            // Evalute User Ranking ->
            //  Evaluate Sentence Difficulty Rank (evaluates previous history and then updates the rank only if it needs updation).
            //  Evaluate Noun Conjugation Rank
            //  Evaluate Verb Conjugation Rank
            //  

            // Determine Next CourseLocation ->
            //  TransitionToNextLocation ->
            //    if currLoc == LessonHub then 
            //      // TODO: Do sub cases here. Update topic state in case of new topic to relevant topic section.
            //    else if currLoc == Topic then
            //      if CurrentSection is Intro then
            //         check to see if anymore slides i.e intro complete?
            //           if not complete then update the index
            //           if complete then mark intro complete, update section to Exercise
            //      if CurrentSection is Exercise then
            //          check to see if in the middle of a pack.
            //          if yes then set next question dimension.
            //          if no then 
            //             if is Exercise complete?
            //             move to Review
            //             else move to next pack. (reset isquestion to false, questiondimension to unknown, etc)
            //      if CurrentSection is Review then
            //         check to see if review complete?
            //         if yes move to extras
            //         else update review question counters or something. (basically remain in review).
            //      if CurrentSection is Extras then
            //         Evaluate extras hub action from the response. (response from client is available here).
            //          if Depth selection move to Depth
            //          else (nothing to do)
            //      if CurrentSection is Depth then
            //         check to see if anymore slides i.e depth complete?
            //           if not complete then update the index
            //           if complete then mark depth complete
            //    if isTopicComplete then
            //     Update Weaknesses, 
            //     Move current topic to history, 
            //     Determine next topic, 
            //     Set location to LessonHub

            return Json(null);
        }
    }
}
