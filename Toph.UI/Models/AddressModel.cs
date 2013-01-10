using System;
using System.ComponentModel.DataAnnotations;

namespace Toph.UI.Models
{
    public class AddressModel
    {
        public AddressModel()
        {
            Line1 = "";
            Line2 = "";
            City = "";
            State = "";
            PostalCode = "";
        }

        public AddressModel(string line1, string line2, string city, string state, string postalCode)
        {
            Line1 = line1;
            Line2 = line2;
            City = city;
            State = state;
            PostalCode = postalCode;
        }

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
