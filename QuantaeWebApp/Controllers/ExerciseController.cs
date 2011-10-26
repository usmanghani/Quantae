using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quantae.Engine;
using Quantae.DataModel;
using Quantae.ViewModels;

namespace QuantaeWebApp.Controllers
{
    public class ExerciseController : Controller
    {
        //
        // GET: /Exercise/

        public ActionResult Index()
        {
            ISentenceSelectionEngine engine = new SentenceSelectionEngine();
            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);
            GetNextSentenceResult result = engine.GetNextSentence(profile);

            ExerciseViewModel model = new ExerciseViewModel();

            if (result.Success)
            {
                                
            }
            return View(ViewNames.Exercise.ExerciseIndexView, model);
        }

    }
}
