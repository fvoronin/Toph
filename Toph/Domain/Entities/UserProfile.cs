using System;
using System.Collections.Generic;
using System.Linq;
using Toph.Common;

namespace Toph.Domain.Entities
{
    public class UserProfile : EntityBase
    {
        protected UserProfile()
        {
        }

        internal UserProfile(string username)
        {
            Username = username;
        }

        private readonly IList<Invoice> _invoices = new List<Invoice>();

        public virtual string Username { get; protected set; }

        public virtual IReadOnlyList<Invoice> Invoices
        {
            get { return _invoices.AsReadOnly(); }
        }

        public virtual Invoice CreateNewInvoice()
        {
            var invoiceDate = DateTimeOffset.Now;
            var invoiceNumber = _invoices.Count(x => x.InvoiceDate.UtcDateTime.Date == invoiceDate.UtcDateTime.Date) + 1;

            var invoice = new Invoice(this, invoiceDate, "{0:yyyyMMdd}{1:d3}".F(invoiceDate, invoiceNumber));

            _invoices.Add(invoice);

            return invoice;
        }

        public virtual void Remove(Invoice invoice)
        {
            _invoices.Remove(invoice);
        }
    }
}
