using System;
using System.Linq;
using System.Web.Mvc;
using Toph.UI.Models;

namespace Toph.UI.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Load()
        {
            var random = new Random();

            var model = new[]
                        {
                            RandomInvoice(DateTime.Now.AddDays(-random.Next(1, 100))),
                            RandomInvoice(DateTime.Now.AddDays(-random.Next(1, 100)))
                        }
                .OrderByDescending(x => x.InvoiceDate)
                .ToArray();

            return PartialView("_Load", model);
        }

        public ActionResult Add()
        {
            var model = RandomInvoice(DateTime.Now);

            return PartialView("_Invoice", model);
        }

        private InvoicesInvoiceModel RandomInvoice(DateTime invoiceDate)
        {
            return new InvoicesInvoiceModel
                   {
                       InvoiceDate = invoiceDate.ToShortDateString(),
                       InvoiceNumber = invoiceDate.ToString("yyyyMMdd"),
                       CustomerName = "John Doe, Inc.",
                       Address = new AddressModel {Line1 = "123 Main St.", City = "Fayetteville", State = "AR", PostalCode = "72703"},
                       InvoiceTotal = "$3,500",
                       LineItems = new[]
                                   {
                                       new InvoicesInvoiceModel.LineItem
                                       {
                                           LineItemDate = invoiceDate.ToShortDateString(),
                                           Description = "Project XYZ",
                                           Quantity = "1",
                                           Amount = "$3,500",
                                           LineItemTotal = "$3,500"
                                       }
                                   }
                   };
        }
    }
}
