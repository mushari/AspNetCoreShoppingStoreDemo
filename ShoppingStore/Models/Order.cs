using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models
{
    public class Order
    {
        [BindNever]
        public int OrderId { get; set; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        [BindNever]
        public bool Shipped { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        [Display(Name = "AddressLine")]
        public string Line1 { get; set; }
        [Display(Name = "AddressLine")]
        public string Line2 { get; set; }
        [Display(Name = "AddressLine")]
        public string Line3 { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "Zip")]
        public string Zip { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        [Display(Name = "Country")]
        public string Country { get; set; }
    }
}
