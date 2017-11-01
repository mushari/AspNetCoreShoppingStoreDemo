using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models
{
    public class Category
    {
        [Required(ErrorMessage = "RequiredError")]
        public string CategoryId { get; set; }
        [Required(ErrorMessage = "RequiredError")]
        public string CategoryName { get; set; }
    }

}
