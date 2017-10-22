using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.Dto
{
    public class CategoryDto
    {
        [Required]
        public string CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
