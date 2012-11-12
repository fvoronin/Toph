using System;
using System.Collections.Generic;
using Toph.Common;

namespace Toph.Domain.Entities
{
    public class Invoice : EntityBase
    {
        protected Invoice()
        {
        }

        internal Invoice(Customer customer, DateTimeOffset invoiceDate)
        {
            Customer = customer;
            InvoiceDate = invoiceDate;
        }

        private readonly IList<InvoiceLineItem> _lineItems = new List<InvoiceLineItem>();

        public virtual Customer Customer { get; protected set; }
        public virtual DateTimeOffset InvoiceDate { get; protected set; }

        public virtual IReadOnlyList<InvoiceLineItem> LineItems
        {
            get { return _lineItems.AsReadOnly(); }
        }
    }

    public class InvoiceLineItem : EntityBase
    {
        protected InvoiceLineItem()
        {
        }

        internal InvoiceLineItem(Invoice invoice)
        {
            Invoice = invoice;
        }

        private readonly IList<TimeEntry> _timeEntries = new List<TimeEntry>();

        public virtual Invoice Invoice { get; protected set; }
        public virtual DateTimeOffset LineItemDate { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual double Quantity { get; protected set; }
        public virtual double Price { get; protected set; }

        public virtual IReadOnlyList<TimeEntry> TimeEntries
        {
            get { return _timeEntries.AsReadOnly(); }
        }
    }
}
