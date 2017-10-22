using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingStore.Models.ProductViewModels;
using ShoppingStore.Data.Repositories;
using PaginationTagHelper.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using ShoppingStore.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository productRepository;
        private IPhotoRepository photoRepository;
        private ICategoryRepository categoryRepository;
        private IOptions<RequestLocalizationOptions> options;
        private IMapper mapper;
        private IUnitOfWork uow;
        public ProductController(
            IProductRepository productRepository,
            IPhotoRepository photoRepository,
            ICategoryRepository categoryRepository,
            IOptions<RequestLocalizationOptions> options,
            IMapper mapper,
            IUnitOfWork uow)
        {
            this.productRepository = productRepository;
            this.photoRepository = photoRepository;
            this.categoryRepository = categoryRepository;
            this.options = options;
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

            var requestFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var currentCulture = requestFeature.RequestCulture.Culture.Name;

            //var categories = categoryRepository.GetCategories()
            //    .Where(c => c.CategoryId.EndsWith("_" + currentCulture))
            //    .ToList();


            //var product = new AddProductViewModel
            //{
            //    Photos = photoRepository.GetPhotos().ToList(),
            //    Categories = categories
            //};

            return View(CategoriesAndPhotosList(currentCulture));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddProductViewModel productViewModel)
        {

            var requestFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var currentCulture = requestFeature.RequestCulture.Culture.Name;

            if (!ModelState.IsValid)
            {
                return View(CategoriesAndPhotosList(currentCulture));
            }


            var products = productRepository.GetProducts().Where(p=>p.ProductId.Contains(currentCulture)).ToList();
            foreach (var product in products)
            {
                if (productViewModel.ProductId == product.ProductId.Split("_")[0])
                {
                    var categories = categoryRepository.GetCategories()
                    .Where(c => c.CategoryId.EndsWith("_" + currentCulture))
                    .ToList();

                    var product_error = new AddProductViewModel
                    {
                        Photos = photoRepository.GetPhotos().ToList(),
                        Categories = categories
                    };
                    ModelState.AddModelError("", "Product Id has already had.");
                    return View(product_error);
                } 
           }

            var cultures = options.Value.SupportedCultures;
            foreach (var culture in cultures)
            {
                Product newproduct = new Product
                {
                    ProductId = productViewModel.ProductId + "_" + culture.Name,
                    CategoryId = productViewModel.CategoryId + "_" + culture.Name,
                    PhotoId = productViewModel.PhotoId,
                    Name = productViewModel.Name,
                    Description = productViewModel.Description,
                    Price = productViewModel.Price,
                };
                productRepository.AddProduct(newproduct);
            }

            await uow.CompleteAsync();

            return RedirectToAction("Index");
        }


        [HttpDelete]
        public IActionResult DeleteProduct(string productId)
        {
            var requestFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var currentCulture = requestFeature.RequestCulture.Culture.Name;

            var product = productRepository.GetProduct(productId, currentCulture);

            productRepository.RemoveProduct(product);

            return Ok("Delete Product" + product.ProductId + "Success");
        }

        private AddProductViewModel CategoriesAndPhotosList(string currentCulture)
        {
            var categories = categoryRepository.GetCategories()
                    .Where(c => c.CategoryId.EndsWith("_" + currentCulture))
                    .ToList();

            var product = new AddProductViewModel
            {
                Photos = photoRepository.GetPhotos().ToList(),
                Categories = categories
            };

            return product;
        }

    }
}
