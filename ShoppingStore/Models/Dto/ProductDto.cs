using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.Dto
{
    public class ProductDto
    {

        public string ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }


        public int PhotoId { get; set; }
        public IEnumerable<Photo> Photos { get; set; }

        public int ProductCategoryId { get; set; }

        public decimal RatingStar { get; set; }

        public string Culture { get; set; }
    }
}