using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }

        public ICollection<SubCategory> SubCategories { get; set; }
        public Category()
        {
            SubCategories = new Collection<SubCategory>();
        }
    }
}
