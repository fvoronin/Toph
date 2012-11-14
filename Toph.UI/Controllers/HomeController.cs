using System;
using System.Web.Mvc;

namespace Toph.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated)
                return View("IndexUnauthenticated");

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
