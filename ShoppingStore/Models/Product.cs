using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models
{
    public class Product
    {
        [Required(ErrorMessage = "RequiredError")]
        [Display(Name = "ProductId")]
        public string ProductId { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        [Display(Name = "ProductName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        [Display(Name = "ProductDescription")]
        public string Description { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        [Display(Name = "ProductPrice")]
        public decimal Price { get; set; }


        [Required(ErrorMessage = "RequiredError")]
        public string PhotoId { get; set; }
        public Photo Photo { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        public string CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
