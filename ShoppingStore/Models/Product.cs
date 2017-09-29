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
        [Display(Name="Product ID")]
        public int ProductID { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name="Product Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name="Product Price")]
        public decimal Price { get; set; }
        [Required]
        [Display(Name="Product Category")]
        public string Category { get; set; }
    }
}
