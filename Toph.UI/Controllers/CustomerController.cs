using System;
using System.Web.Mvc;
using Toph.UI.Models;

namespace Toph.UI.Controllers
{
    public class CustomerController : AppController
    {
        public ActionResult Index(string username, string customer)
        {
            return View(new CustomerIndexModel{Username = username, CustomerName = customer});
        }
    }
}
