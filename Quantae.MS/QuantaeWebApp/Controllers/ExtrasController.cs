using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quantae.DataModel;
using Quantae.Engine;
using Quantae.ViewModels;
using System.Diagnostics;
using Quantae.Repositories;

namespace QuantaeWebApp.Controllers
{
    public class ExtrasController : Controller
    {
        //
        // GET: /Extras/
        [Authorize]
        public ActionResult Index()
        {
            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);
            Topic topic = TopicOperations.GetTopicFromHandle(profile.CurrentState.CourseLocationInfo.CurrentTopic.Topic);
            Hub h = topic.ExtrasHub;
            ExtrasHubViewModel model = new ExtrasHubViewModel();
            foreach(var action in h.Actions)
            {
                model.AddAction(action);
            }

            return View(ViewNames.Extras.ExtrasHubView, model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Select(ExtrasHubResponseModel responseModel)
        {
            ActionResult result = RedirectToAction("Index");
            UserProfile profile = UserOperations.GetUserProfileFromSession(User.Identity.Name);
            
            if (ModelState.IsValid)
            {
                switch (responseModel.Choice)
                {
                    case "Depth":
                        profile.CurrentState.CourseLocationInfo.TopicLocationInfo.CurrentSection = TopicSectionType.Depth;
                        result = RedirectToAction("Index", "Depth");
                        Repositories.Users.Save(profile);
                        break;
                    case "MoveOn":
                        result = RedirectToAction("Next", "Section");
                        break;
                    default:
                        Trace.TraceError("Unsupported choice on the extras hub", responseModel.Choice);
                        break;
                }
            }

            return result;
        }
    }
}
