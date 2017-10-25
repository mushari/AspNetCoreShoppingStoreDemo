using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.ProductViewModels
{
    public class EditProductViewModel
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
        [Display(Name = "ProductPrice")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "ProductPhoto")]
        public IFormFile Photo { get; set; }
        public string PhototId { get; set; }

        [Required]
        [Display(Name = "ProductCategoryId")]
        public string CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
