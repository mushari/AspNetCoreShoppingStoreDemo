using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.Dto
{
    public class ProductDto
    {
        public int ProductID { get; set; }

        public int TranslationId { get; set; }

        public string Culture { get; set; }

        public string PhotoId { get; set; }
        public IEnumerable<Photo> Photos { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime PublishedDate { get; set; }

        public decimal Price { get; set; }

        public int CategoryID { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public decimal RatingStar { get; set; }
    }
}