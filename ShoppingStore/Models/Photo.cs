using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models
{
    public class Photo
    {
        [Required(ErrorMessage = "RequiredError")]
        public string PhotoId { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required(ErrorMessage = "RequiredError")]
        public string FileAddress { get; set; }
    }
}
