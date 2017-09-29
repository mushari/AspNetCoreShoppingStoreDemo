using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingStore.Models;

namespace ShoppingStore.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationDbContext context;
        public ProductRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Product> Products =>
            context.Products;

        public void AddProduct(Product product) =>
            context.Products.Add(product);

        public async Task AddProductAsync(Product product) =>
            await context.Products.AddAsync(product);

        public void AddProducts(IEnumerable<Product> products) =>
            context.Products.AddRange(products);

        public async Task AddProductsAsync(IEnumerable<Product> products) =>
            await context.Products.AddRangeAsync(products);


        public void RemoveProduct(string id)
        {
            var product = context.Products.Find(id);
            if (product != null)
            {
                context.Products.Remove(product);
            }
        }

        public void RemoveProducts(IEnumerable<string> ids)
        {
            foreach (var id in ids)
            {
                var product = context.Products.Find(id);
                if (product != null)
                {
                    context.Products.Remove(product);
                }
            }
        }


    }
}
