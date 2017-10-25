using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingStore.Data.Repositories;
using Microsoft.AspNetCore.Localization;
using ShoppingStore.Models;
using ShoppingStore.Models.Dto;
using Microsoft.AspNetCore.Builder;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;

namespace ShoppingStore.Controllers
{
    public class CategoryController : Controller
    {
        private ICategoryRepository categoryRepository;
        private IOptions<RequestLocalizationOptions> options;
        private IUnitOfWork uow;
        public CategoryController(
            ICategoryRepository categoryRepository,
            IOptions<RequestLocalizationOptions> options,
            IUnitOfWork uow)
        {
            this.categoryRepository = categoryRepository;
            this.options = options;
            this.uow = uow;
        }

        [HttpGet]
        public IActionResult Index(string url)
        {
            var categories = categoryRepository.GetCategories().ToList();
            ViewBag.ReturnUrl = url;

            return View(categories);
        }

        [HttpGet]
        [Route("api/getCategories")]
        public IActionResult GetCategories()
        {
            var requestFeature = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            var currentCulture = requestFeature.RequestCulture.Culture.Name;

            var categories = categoryRepository.GetCategories()
                .Where(c => c.CategoryId.EndsWith("_" + currentCulture)).ToList();

            return Ok(categories);
        }

        [HttpPost]
        [Route("/api/addCategory")]
        public IActionResult AddCategory(CategoryDto categoryDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var getCategories = categoryRepository.GetCategories().ToList();

            foreach (var category in getCategories)
            {
                if (category.CategoryId.Split("_")[0] == (categoryDto.CategoryId))
                {
                    return BadRequest(new { repeatedId = "CategoryId has repeated" });
                }
            }

            var cultures = options.Value.SupportedCultures;
            foreach (var culture in cultures)
            {
                var category = new Category
                {
                    CategoryId = categoryDto.CategoryId + "_" + culture.Name,
                    CategoryName = categoryDto.CategoryName,
                };
                categoryRepository.AddCategory(category);
            }
            uow.Complete();

            return Ok();
        }

        [HttpPost]
        [Route("api/editCategory")]
        public IActionResult EditCategory(string pk, string value)
        {
            var category = categoryRepository.GetCategory(pk);
            if (category == null)
            {
                return NotFound();
            }
            category.CategoryName = value;
            uow.Complete();
            return Ok("Edit Successful");
        }

        [HttpDelete]
        [Route("api/deleteCategory")]
        public IActionResult DeleteCategory(string pk)
        {
            var categoryId = pk.Split("_")[0];
            var categories = categoryRepository.GetCategories()
                .Where(c => c.CategoryId.Split("_")[0] == (categoryId)).ToList();

            if (categories == null)
            {
                return NotFound();
            }
            foreach (var category in categories)
            {
                categoryRepository.DeleteCategory(category);
            }

            uow.Complete();

            return Ok("Delete Successful");
        }
    }
}