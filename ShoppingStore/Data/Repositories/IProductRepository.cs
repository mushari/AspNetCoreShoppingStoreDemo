using ShoppingStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Data.Repositories
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetProducts();
        IEnumerable<Product> GetProductsWithCategory();
        IEnumerable<Product> GetProductsWithPhoto();
        IEnumerable<Product> GetProductsWithAll();

        Product GetProduct(string id);
        Product GetProductWithCategory(string id);
        Product GetProductWithPhoto(string id);
        Product GetProductWithAll(string id);

        void AddProduct(Product product);
        Task AddProductAsync(Product product);
        void RemoveProduct(Product product);


        void AddProducts(IEnumerable<Product> products);
        Task AddProductsAsync(IEnumerable<Product> products);
        void RemoveProducts(IEnumerable<Product> products);

    }
}
