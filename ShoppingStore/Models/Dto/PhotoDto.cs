using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.Dto
{
    public class PhotoDto
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }
}
