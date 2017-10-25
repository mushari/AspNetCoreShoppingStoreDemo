using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv.Internal.Networking;
using System.Globalization;

namespace ShoppingStore.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private ApplicationDbContext context;
        public ProductRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Product> GetProducts()
        {
            return context.Products.Include(p => p.Category);
        }

        public Product GetProduct(string id)
        {
            return context.Products.Include(p => p.Category).SingleOrDefault(
                p => p.ProductId == id);
        }


        public void AddProduct(Product product)
        {
            context.Products.Add(product);
        }

        public async Task AddProductAsync(Product product)
        {
            await context.Products.AddAsync(product);
        }

        public void AddProducts(IEnumerable<Product> products)
        {
            context.Products.AddRange(products);
        }

        public async Task AddProductsAsync(IEnumerable<Product> products)
        {
            await context.Products.AddRangeAsync(products);
        }


        public void RemoveProduct(Product product)
        {
            context.Products.Remove(product);
        }

        public void RemoveProducts(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                context.Products.Remove(product);
            }
        }

    }
}
