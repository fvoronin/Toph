using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using Toph.Domain.Entities;

namespace Toph.UI.Models
{
    public class InvoicesInvoiceModel
    {
        public InvoicesInvoiceModel()
        {
        }

        public InvoicesInvoiceModel(Invoice invoice)
        {
            InvoiceId = invoice.Id;
            InvoiceDate = invoice.InvoiceDate.ToString("MM/dd/yyyy");
            InvoiceNumber = invoice.InvoiceNumber;
            InvoiceLineItems = invoice.LineItems.Select(x => new LineItem(x)).ToArray();
            InvoiceCustomer = new Customer(invoice.InvoiceCustomer);
        }

        public int InvoiceId { get; set; }

        [Required]
        public string InvoiceDate { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        public Customer InvoiceCustomer { get; set; }

        public LineItem[] InvoiceLineItems { get; set; }

        public class Customer
        {
            public Customer()
            {
            }

            public Customer(InvoiceCustomer customer)
            {
                Name = customer == null ? "Costomer not set" : customer.Name;
                Address = customer == null ? new AddressModel() : new AddressModel(customer.Line1, customer.Line2, customer.City, customer.State, customer.PostalCode);
            }

            [Required]
            public string Name { get; set; }

            [Required]
            public AddressModel Address { get; set; }
        }

        public class LineItem
        {
            public LineItem()
            {
            }

            public LineItem(InvoiceLineItem lineItem)
            {
                LineItemId = lineItem.Id;
                LineItemDate = lineItem.LineItemDate.ToString("MM/dd/yyyy");
                Description = lineItem.Description;
                Quantity = lineItem.Quantity.ToString(CultureInfo.InvariantCulture);
                Price = lineItem.Price.ToString("C");
                LineItemTotal = lineItem.GetTotal().ToString("C");
            }

            public int LineItemId { get; set; }

            [Required]
            public string LineItemDate { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public string Quantity { get; set; }

            [Required]
            public string Price { get; set; }

            [Required]
            public string LineItemTotal { get; set; }
        }
    }
}
