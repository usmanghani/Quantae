using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestMvcApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult jsontest()
        {
            TestMvcApp.Models.TestModel model = new Models.TestModelDerived();
            model.ContextualAnalysis.Add(new Models.ContextualAnalysis { startIndex = new int[] { 5 }, endIndex = new int[] { 4 } });
            model.GrammarEntries.Add(new Models.GrammarEntry { Word = "word", Translation = "translation" });

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
