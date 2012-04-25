using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quantae.ViewModels;
using Quantae.DataModel;
using Quantae.Engine;
using System.Diagnostics;
using System.Net;

namespace QuantaeWebApp.Controllers
{
    public class ReviewController : Controller
    {
        //
        // GET: /Review/
        [Authorize]
        public ActionResult Index()
        {
            // TODO: implement Review controller logic here

            ISentenceSelectionEngine engine = new SentenceSelectionEngine();
            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);

            GetNextSentenceResult result = engine.GetNextSentence(profile);

            if (result == null)
            {
                return new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError, "No donut for you!");
            }
            if (!result.IsReview)
            {
                Trace.TraceError("Unexpected sentence result.  Should only receive Review sentences during review flow.");
                return new HttpStatusCodeResult((int)HttpStatusCode.InternalServerError, "You are not supposed to be here!");
            }
            
            ReviewViewModel model = new ReviewViewModel(result.Sentence.SentenceText, result.Sentence.SentenceTranslation);
            return View(ViewNames.Review.ReviewIndexView, model);
        }
    }
}
