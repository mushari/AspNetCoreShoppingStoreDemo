using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models
{
    public class SubCategory
    {
        public string SubCategoryID { get; set; }
        public string SubCategoryName { get; set; }

        public string Culture { get; set; }

        public string CategoryID { get; set; }
        public Category Category { get; set; }
    }
}
