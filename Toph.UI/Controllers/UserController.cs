using System;
using System.Web.Mvc;
using Toph.UI.Models;

namespace Toph.UI.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index(string username)
        {
            return View(new UserIndexModel {Username = username, ShowManageLink = string.Equals(username, User.Identity.Name, StringComparison.OrdinalIgnoreCase)});
        }
    }
}
