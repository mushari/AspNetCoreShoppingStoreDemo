using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models
{
    public class Product
    {
        [Required]
        [Display(Name = "ProductID")]
        public int ProductID { get; set; }

        public Photo Photo { get; set; }
        public string PhotoId { get; set; }

        [Required]
        [Display(Name = "ProductName")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "ProductDescription")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "ProductPublishedDate")]
        public DateTime PublishedDate { get; set; }

        [Required]
        [Display(Name = "ProductPrice")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "ProductCatetory")]
        public int CategoryID { get; set; }

        public Category Category { get; set; }

        public decimal RatingStar { get; set; }

        public string Culture { get; set; }

    }
}
