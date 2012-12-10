using System;
using System.ComponentModel.DataAnnotations;

namespace Toph.UI.Models
{
    public class AddressModel
    {
        [Required]
        public string Line1 { get; set; }

        public string Line2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string PostalCode { get; set; }
    }
}
