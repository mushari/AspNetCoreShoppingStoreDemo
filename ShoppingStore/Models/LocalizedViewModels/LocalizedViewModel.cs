using AspNetCore.JsonLocalization;
using PaginationTagHelper.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingStore.Models.LocalizedViewModels
{
    public class LocalizedViewModel : IPagingObject<JsonLocalizationFormat>
    {
        public int Page { get; set; } = 1;
        public int ItemPerPage { get; set; } = 5;
        public int TotalItems { get; set; }
        public IEnumerable<JsonLocalizationFormat> Items { get; set; }
    }
}
