using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingStore.Models.ProductViewModels;
using ShoppingStore.Data.Repositories;
using PaginationTagHelper.Extensions;
using ShoppingStore.Models.Dto;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ShoppingStore.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository productRepository;
        private IPhotoRepository photoRepository;
        private ICategoryRepository categoryRepository;
        private IMapper mapper;
        private IUnitOfWork uow;
        public ProductController(
            IProductRepository productRepository,
            IPhotoRepository photoRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper,
            IUnitOfWork uow)
        {
            this.productRepository = productRepository;
            this.photoRepository = photoRepository;
            this.categoryRepository = categoryRepository;
            this.mapper = mapper;
            this.uow = uow;
        }


        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index(ProductViewModel model)
        {
            var requestFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var currentCulture = requestFeature.RequestCulture.Culture.Name;

            var products = productRepository.GetProducts().Where(p => p.ProductId.EndsWith("_" + currentCulture)).AsQueryable();

            var photos = photoRepository.GetPhotos().ToList();

            model.ItemPerPage = 5;

            var productsWithPaging = products
                .ToPageList(model.Page, model.ItemPerPage);
                

            ProductViewModel productViewModel = new ProductViewModel
            {
                Items = productsWithPaging,
                Page = model.Page,
                ItemPerPage = model.ItemPerPage,
                TotalItems = products.Count(),
            };

            return View("~/Views/Product/Index.cshtml", productViewModel);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            var product = new ProductDto
            {
                Photos = photoRepository.GetPhotos().ToList(),
                Categories= categoryRepository.GetCategories().ToList()
            };
            return View(product);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductDto productDto)
        {
            var requestFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var currentCulture = requestFeature.RequestCulture.Culture.Name;

            productDto.Culture = currentCulture;

            var product = mapper.Map<ProductDto, Product>(productDto);

            if (product != null)
            {
                productRepository.AddProduct(product);
            }

            return Ok(productDto);
        }

        [HttpDelete]
        public IActionResult DeleteProduct(string productId)
        {
            var requestFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var currentCulture = requestFeature.RequestCulture.Culture.Name;

            var product = productRepository.GetProduct(productId,currentCulture);

            productRepository.RemoveProduct(product);

            return Ok("Delete Product" + product.ProductId + "Success");
        }

    }
}
