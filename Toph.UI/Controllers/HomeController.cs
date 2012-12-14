using System;
using System.Web.Mvc;

namespace Toph.UI.Controllers
{
    public class HomeController : AppController
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
