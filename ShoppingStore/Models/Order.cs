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

        [Required(ErrorMessage = "EnterName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "EnterAddress")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }

        [Required(ErrorMessage = "EnterCity")]
        public string City { get; set; }

        [Required(ErrorMessage = "EnterStateName")]
        public string State { get; set; }

        public string Zip { get; set; }

        [Required(ErrorMessage = "EnterCountry")]
        public string Country { get; set; }
    }
}
