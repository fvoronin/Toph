using System;
using System.Web.Mvc;

namespace Toph.UI.Controllers
{
    public class HomeController : AppController
    {
        public ActionResult Index()
        {
            return Request.IsAuthenticated ? View() : View("IndexUnauthenticated");
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
