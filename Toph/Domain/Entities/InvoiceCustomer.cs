using System;

namespace Toph.Domain.Entities
{
    public class InvoiceCustomer
    {
        public InvoiceCustomer()
        {
        }

        public InvoiceCustomer(string name, string line1, string line2, string city, string state, string postalCode)
        {
            Name = name;
            Line1 = line1;
            Line2 = line2;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

        public virtual string Name { get; set; }
        public virtual string Line1 { get; set; }
        public virtual string Line2 { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string PostalCode { get; set; }
    }
}
