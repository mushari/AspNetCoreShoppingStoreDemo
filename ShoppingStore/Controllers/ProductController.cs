using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingStore.Models.ProductViewModels;
using ShoppingStore.Data.Repositories;
using PaginationTagHelper.Extensions;
using ShoppingStore.Models.Dto;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public ProductController(IProductRepository repository)
        {
            this.repository = repository;
        }


        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index(ProductViewModel model)
        {
            var products = repository.Products.AsQueryable();

            model.ItemPerPage = 5;

            products = products.ToPageList(model.Page, model.ItemPerPage);

            ProductViewModel productViewModel = new ProductViewModel
            {
                Items = products,
                Page = model.Page,
                ItemPerPage = model.ItemPerPage,
                TotalItems = repository.Products.Count()
            };

            return View("~/Views/Product/Index.cshtml", productViewModel);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductDto productDto)
        {
            return Ok();
        }

    }
}
