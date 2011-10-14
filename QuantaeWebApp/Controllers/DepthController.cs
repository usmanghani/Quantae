using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuantaeWebApp.Controllers
{
    public class DepthController : Controller
    {
        //
        // GET: /Depth/

        public ActionResult Index()
        {
            return View();
        }

        // GET: /Depth/NextSlide
        public ActionResult NextSlide()
        {
            return View();
        }
    }
}
