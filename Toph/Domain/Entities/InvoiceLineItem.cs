using System;

namespace Toph.Domain.Entities
{
    public class InvoiceLineItem : EntityBase
    {
        protected InvoiceLineItem()
        {
        }

        internal InvoiceLineItem(Invoice invoice)
        {
            Invoice = invoice;
            LineItemDate = DateTimeOffset.Now;
            Description = "";
            Quantity = 0d;
            Price = 0d;
        }

        public virtual Invoice Invoice { get; protected set; }
        public virtual DateTimeOffset LineItemDate { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual double Quantity { get; protected set; }
        public virtual double Price { get; protected set; }

        public virtual double GetTotal()
        {
            return Quantity * Price;
        }
    }
}