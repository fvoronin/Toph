using System;
using System.Linq;
using System.Web.Mvc;
using Toph.Common.DataAccess;
using Toph.Domain.Entities;
using Toph.UI.Models;

namespace Toph.UI.Controllers
{
    [Authorize]
    public class InvoicesController : AppController
    {
        public InvoicesController(IRepository repository, IUnitOfWork uow)
        {
            _repository = repository;
            _uow = uow;
        }

        private readonly IRepository _repository;
        private readonly IUnitOfWork _uow;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Load()
        {
            var model = _repository
                .Find<Invoice>()
                .Where(x => x.UserProfile.Username == User.Identity.Name)
                .OrderByDescending(x => x.InvoiceDate)
                .Select(x => new InvoicesInvoiceModel(x))
                .ToArray();

            return PartialView("_Load", model);
        }

        public ActionResult Add()
        {
            var user = _repository.Get<UserProfile>(x => x.Username == User.Identity.Name);

            var invoice = user.CreateNewInvoice();
            _uow.Commit();

            return PartialView("_Invoice", new InvoicesInvoiceModel(invoice));
        }
    }
}
