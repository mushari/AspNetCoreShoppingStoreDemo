using PaginationTagHelper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.ProductViewModels
{
    public class ProductViewModel : IPagingObject<Product>
    {
        public int Page { get; set; } = 1;
        public int ItemPerPage { get; set; }
        public int TotalItems { get; set; }

        public IEnumerable<Product> Items { get; set; }

        public Product Product { get; set; }

        public Photo Photo { get; set; }

    }
}
