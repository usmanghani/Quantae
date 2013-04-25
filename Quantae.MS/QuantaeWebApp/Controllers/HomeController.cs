using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;

namespace QuantaeWebApp.Controllers
{
    public class HomeController : Controller
    {
        private ILog logger = null;
        public HomeController(ILoggingService logger)
        {
            this.logger = logger.Logger;
        }

        public ActionResult Index()
        {
            this.logger.Info("home");
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
