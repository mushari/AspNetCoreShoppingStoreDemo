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
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository productRepository;
        private IPhotoRepository photoRepository;
        private ICategoryRepository categoryRepository;
        private IHostingEnvironment host;
        private IOptions<RequestLocalizationOptions> options;
        private IMapper mapper;
        private IUnitOfWork uow;
        public ProductController(
            IProductRepository productRepository,
            IPhotoRepository photoRepository,
            ICategoryRepository categoryRepository,
            IHostingEnvironment host,
            IOptions<RequestLocalizationOptions> options,
            IMapper mapper,
            IUnitOfWork uow)
        {
            this.productRepository = productRepository;
            this.photoRepository = photoRepository;
            this.categoryRepository = categoryRepository;
            this.host = host;
            this.options = options;
            this.mapper = mapper;
            this.uow = uow;
        }


        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index(ProductViewModel model)
        {
            var currentCulture = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            var products = productRepository.GetProducts()
                .Where(p => p.ProductId.EndsWith("_" + currentCulture)).AsQueryable();

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
            var currentCulture = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            var photos = photoRepository.GetPhotos().ToList();
            var categories = categoryRepository.GetCategories()
                    .Where(c => c.CategoryId.EndsWith("_" + currentCulture))
                    .ToList();

            var viewModel = new ProductFormViewModel
            {
                FormType = "Add",
                Photos = photos,
                Categories = categories,
                ReturnUrl = Request.Path.Value
            };

            return View("ProductForm", viewModel);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var currentCulture = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            var product = productRepository.GetProduct(id);
            var photos = photoRepository.GetPhotos().ToList();
            var categories = categoryRepository.GetCategories()
                    .Where(c => c.CategoryId.EndsWith("_" + currentCulture))
                    .ToList();

            if (product == null)
            {
                return View("Error");
            }

            var viewModel = new ProductFormViewModel
            {
                FormType = "Edit",
                ProductId = product.ProductId.Split("_")[0],
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                PhotoId = product.PhotoId,
                Photos = photos,
                Categories = categories,
                ReturnUrl = Request.Path.Value
            };

            return View("ProductForm", viewModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProduct(ProductFormViewModel productViewModel)
        {
            var currentCulture = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            var photos = photoRepository.GetPhotos().ToList();
            var categories = categoryRepository.GetCategories()
                    .Where(c => c.CategoryId.EndsWith("_" + currentCulture))
                    .ToList();

            if (productViewModel.FormType == "Add")
            {

                if (!ModelState.IsValid)
                {
                    var viewModel = new ProductFormViewModel
                    {
                        FormType = "Add",
                        Photos = photos,
                        Categories = categories,
                        ReturnUrl = Request.Path.Value
                    };
                    return View("ProductForm", viewModel);
                }

                var products = productRepository.GetProducts().Where(
                    p => p.ProductId.Contains(currentCulture)).ToList();

                if (products.Any(p => p.ProductId.Split("_")[0]
                        .Contains(productViewModel.ProductId)))
                {
                    var viewModel = new ProductFormViewModel
                    {
                        FormType = productViewModel.FormType,
                        Photos = photos,
                        Categories = categories,
                        ReturnUrl = Request.Path.Value
                    };
                    ModelState.AddModelError("", "Product Id has already had.");
                    return View("ProductForm", viewModel);
                }

                var cultures = options.Value.SupportedCultures;
                foreach (var culture in cultures)
                {
                    Product newproduct = new Product
                    {
                        ProductId = productViewModel.ProductId + "_" + culture,
                        CategoryId = productViewModel.CategoryId + "_" + culture,
                        PhotoId = productViewModel.PhotoId,
                        Name = productViewModel.Name,
                        Description = productViewModel.Description,
                        Price = productViewModel.Price,
                    };
                    productRepository.AddProduct(newproduct);
                }
                await uow.CompleteAsync();

            }
            else if (productViewModel.FormType == "Edit")
            {
                var product = productRepository.GetProduct(productViewModel.ProductId + "_" + currentCulture);

                if (product == null)
                {
                    var viewModel = new ProductFormViewModel
                    {
                        FormType = productViewModel.FormType,
                        ProductId = product.ProductId.Split("_")[0],
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        PhotoId = product.PhotoId,
                        Photos = photos,
                        CategoryId = product.CategoryId,
                        Categories = categories,
                        ReturnUrl = Request.Path.Value
                    };
                    ModelState.AddModelError("", "Product doesn't exist.");
                    return View("ProductForm", viewModel);
                }
                if (!ModelState.IsValid)
                {
                    var viewModel = new ProductFormViewModel
                    {
                        FormType = "Edit",
                        ProductId = product.ProductId.Split("_")[0],
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        PhotoId = product.PhotoId,
                        Photos = photos,
                        CategoryId = product.CategoryId,
                        Categories = categories,
                        ReturnUrl = Request.Path.Value
                    };
                    return View("ProductForm", viewModel);
                }


                product.Name = productViewModel.Name;
                product.Description = productViewModel.Description;
                product.Price = productViewModel.Price;
                product.PhotoId = productViewModel.PhotoId;
                product.CategoryId = productViewModel.CategoryId + "_" + currentCulture;

                await uow.CompleteAsync();
            }

            return RedirectToAction("Index");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(string id)
        {
            var product = productRepository.GetProduct(id);

            productRepository.RemoveProduct(product);
            uow.Complete();

            return RedirectToAction("Index");
        }

        //private ProductFormViewModel ShowFormList(string formType, string id)
        //{

        //    var requestFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
        //    var currentCulture = requestFeature.RequestCulture.Culture.Name;

        //    var photos = photoRepository.GetPhotos().ToList();
        //    var categories = categoryRepository.GetCategories()
        //            .Where(c => c.CategoryId.EndsWith("_" + currentCulture))
        //            .ToList();

        //    if (formType == "Add")
        //    {
        //        var viewModel = new ProductFormViewModel
        //        {
        //            FormType = formType,
        //            Photos = photos,
        //            Categories = categories,
        //            ReturnUrl = Request.Path.Value
        //        };

        //        return viewModel;
        //    }
        //    else
        //    {
        //        var product = productRepository.GetProduct(id);
        //        var viewModel = new ProductFormViewModel
        //        {
        //            FormType = formType,
        //            ProductId = product.ProductId.Split("_")[0],
        //            Name = product.Name,
        //            Description = product.Description,
        //            Price = product.Price,
        //            PhotoId = product.PhotoId,
        //            Photos = photos,
        //            Categories = categories,
        //            ReturnUrl = Request.Path.Value
        //        };

        //        return viewModel;
        //    }

        //}

    }
}
