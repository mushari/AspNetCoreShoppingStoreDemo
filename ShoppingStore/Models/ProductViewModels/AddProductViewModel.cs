using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models
{
    public class AddProductViewModel
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

        public IEnumerable<Photo> Photos { get; set; }
        public string PhotoId { get; set; }

        [Required]
        [Display(Name = "ProductCategoryId")]
        public string CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }



    }
}