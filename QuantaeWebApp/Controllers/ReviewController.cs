using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quantae.ViewModels;
using Quantae.DataModel;
using Quantae.Engine;
using System.Diagnostics;

namespace QuantaeWebApp.Controllers
{
    public class ReviewController : Controller
    {
        //
        // GET: /Review/

        public ActionResult Index()
        {
            // TODO: implement Review controller logic here

            ISentenceSelectionEngine engine = new SentenceSelectionEngine();
            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);

            GetNextSentenceResult result = engine.GetNextSentence(profile);

            if (!result.IsReview)
            {
                Trace.TraceError("Unexpected sentence result.  Should only receive Review sentences during review flow.");
            }
            
            ReviewViewModel model = new ReviewViewModel(result.Sentence.SentenceText, result.Sentence.SentenceTranslation);
            return View(ViewNames.Review.ReviewIndexView, model);
        }
    }
}
