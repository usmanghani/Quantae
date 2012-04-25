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
        private static readonly ILog log = LogManager.GetLogger(typeof(HomeController).Name);
        public ActionResult Index()
        {
            log.Info("this is awesome");
            foreach (var i in Enumerable.Range(1, 100))
            {
                log.Info(string.Format("really sire {0}", i));
            }
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
