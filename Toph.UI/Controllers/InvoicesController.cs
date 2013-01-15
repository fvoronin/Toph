using System;
using System.Linq;
using System.Web.Mvc;
using Toph.Common.DataAccess;
using Toph.Domain;
using Toph.Domain.Commands;
using Toph.Domain.Entities;
using Toph.UI.Models;

namespace Toph.UI.Controllers
{
    [Authorize]
    public class InvoicesController : AppController
    {
        public InvoicesController(IUnitOfWork uow, IRepository repository, ICommandExecutor commandExecutor)
        {
            _repository = repository;
            _commandExecutor = commandExecutor;
            _uow = uow;
        }

        private readonly IUnitOfWork _uow;
        private readonly IRepository _repository;
        private readonly ICommandExecutor _commandExecutor;

        public ActionResult Index()
        {
            var model = _repository
                .Find<Invoice>()
                .Where(x => x.UserProfile.Username == User.Identity.Name)
                .OrderByDescending(x => x.InvoiceNumber)
                .Select(x => new InvoicesInvoiceModel(x))
                .ToArray();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Add()
        {
            var user = _repository.Get<UserProfile>(x => x.Username == User.Identity.Name);
            if (user == null) return HttpNotFound();

            var invoice = user.CreateNewInvoice();
            _uow.Commit();

            return Json(new InvoicesInvoiceModel(invoice));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Remove(int invoiceId)
        {
            var user = _repository.Get<UserProfile>(x => x.Username == User.Identity.Name);
            if (user == null) return HttpNotFound();

            var invoice = user.Invoices.SingleOrDefault(x => x.Id == invoiceId);
            if (invoice == null) return HttpNotFound();

            user.Remove(invoice);
            _uow.Commit();

            return new EmptyResult();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult AddLineItem(int invoiceId)
        {
            var user = _repository.Get<UserProfile>(x => x.Username == User.Identity.Name);
            if (user == null) return HttpNotFound();

            var invoice = user.Invoices.SingleOrDefault(x => x.Id == invoiceId);
            if (invoice == null) return HttpNotFound();

            var lineItem = invoice.CreateNewLineItem();
            _uow.Commit();

            return Json(new InvoicesInvoiceModel.LineItem(lineItem));
        }

        public ActionResult CustomerEditForm(int invoiceId)
        {
            var user = _repository.Get<UserProfile>(x => x.Username == User.Identity.Name);
            if (user == null) return HttpNotFound();

            var invoice = user.Invoices.SingleOrDefault(x => x.Id == invoiceId);
            if (invoice == null) return HttpNotFound();

            return PartialView(new InvoicesInvoiceModel.Customer(invoice.InvoiceCustomer ?? new InvoiceCustomer()));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult CustomerEditForm(int invoiceId, InvoicesInvoiceModel.Customer model)
        {
            if (!ModelState.IsValid)
                return PartialView(model);

            var user = _repository.Get<UserProfile>(x => x.Username == User.Identity.Name);
            if (user == null) return HttpNotFound();

            var invoice = user.Invoices.SingleOrDefault(x => x.Id == invoiceId);
            if (invoice == null) return HttpNotFound();

            invoice.UpdateCustomer(model.Name, model.Address.Line1, model.Address.Line2, model.Address.City, model.Address.State, model.Address.PostalCode);
            _uow.Commit();

            return Json(model);
        }

        public ActionResult LineItemEditForm(int id)
        {
            var lineItem = _repository.Get<InvoiceLineItem>(id);
            if (!string.Equals(lineItem.Invoice.UserProfile.Username, User.Identity.Name, StringComparison.OrdinalIgnoreCase)) return HttpNotFound();

            return PartialView(new EditInvoiceLineItemCommand(lineItem));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult LineItemEditForm(EditInvoiceLineItemCommand command)
        {
            var lineItem = _repository.Get<InvoiceLineItem>(command.Id);
            if (!string.Equals(lineItem.Invoice.UserProfile.Username, User.Identity.Name, StringComparison.OrdinalIgnoreCase)) return HttpNotFound();

            var result = _commandExecutor.Execute(command);

            if (result.AnyErrors())
            {
                ModelState.AddModelErrors(result);
                return PartialView(command);
            }

            _uow.Commit();

            return Json(new InvoicesInvoiceModel.LineItem(lineItem));
        }
    }
}
