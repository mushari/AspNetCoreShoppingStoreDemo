using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models
{
    public class Photo
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        public string FileAddress { get; set; }
    }
}
