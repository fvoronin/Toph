using System;
using System.ComponentModel.DataAnnotations;

namespace Toph.UI.Models
{
    public class InvoicesInvoiceModel
    {
        [Required]
        public string InvoiceDate { get; set; }

        [Required]
        public string InvoiceNumber { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public AddressModel Address { get; set; }

        [Required]
        public string InvoiceTotal { get; set; }

        public LineItem[] LineItems { get; set; }

        public class LineItem
        {
            [Required]
            public string LineItemDate { get; set; }

            [Required]
            public string Description { get; set; }

            [Required]
            public string Quantity { get; set; }

            [Required]
            public string Amount { get; set; }

            [Required]
            public string LineItemTotal { get; set; }
        }
    }
}
