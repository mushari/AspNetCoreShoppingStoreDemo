using ShoppingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Data.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> Products { get; }

        void AddProduct(Product product);
        Task AddProductAsync(Product product);
        void RemoveProduct(string id);


        void AddProducts(IEnumerable<Product> products);
        Task AddProductsAsync(IEnumerable<Product> products);
        void RemoveProducts(IEnumerable<string> ids);

    }
}
