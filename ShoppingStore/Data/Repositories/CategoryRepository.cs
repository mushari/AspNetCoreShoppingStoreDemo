using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ShoppingStore.Models;

namespace ShoppingStore.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private ApplicationDbContext context;
        public CategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task AddCategory(Category category)
        {
            await context.Categories.AddAsync(category);
        }

        public void DeleteCategory(Category category)
        {
            context.Categories.Remove(category);
        }

        public IEnumerable<Category> GetCategories()
        {
            return context.Categories;
        }

        public Category GetCategory(string categoryId)
        {
            return context.Categories.SingleOrDefault(c => c.CategoryId == categoryId);
        }
    }
}
