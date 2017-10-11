using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models
{
    public class SubCategory
    {
        public int SubCategoryID { get; set; }
        public string SubCategoryName { get; set; }

        public int CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
