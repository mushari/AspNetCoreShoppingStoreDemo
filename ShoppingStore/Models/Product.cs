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
        [Required]
        [Display(Name = "ProductId")]
        public string ProductId { get; set; }

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
        public string CategoryId { get; set; }

        public Photo Photo { get; set; }
        public int PhotoId { get; set; }

        [Required]
        public string ProductCategoryId { get; set; }
        public Category ProductCategory { get; set; }

        public decimal RatingStar { get; set; }

    }
}
