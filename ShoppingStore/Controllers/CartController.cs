using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingStore.Data.Repositories;
using ShoppingStore.Models;
using ShoppingStore.Infrastructures.Extensions;
using ShoppingStore.Models.CartViewModels;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ShoppingStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductRepository productRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private IOptions<RequestLocalizationOptions> options;
        private Cart cart;

        public CartController(
            IProductRepository productRepository,
            UserManager<ApplicationUser> userManager,
            IOptions<RequestLocalizationOptions> options,
            Cart cartService)
        {
            this.productRepository = productRepository;
            this.userManager = userManager;
            this.options = options;
            cart = cartService;
        }


        [HttpGet]
        public async Task<IActionResult> AddToCart(
            string productId, int quantity, string returnUrl)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var id = productId.Split("_")[0];
            var cultures = options.Value.SupportedCultures;
            foreach (var culture in cultures)
            {
                Product product = productRepository.GetProductWithAll(
                    id + "_" + culture.Name);

                if (product != null)
                {
                    cart.AddItem(product, user, quantity);
                }
            }


            //return RedirectToAction("Index", new { returnUrl });
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveItem(string productId, int quantity, string returnUrl)
        {
            var id = productId.Split("_")[0];
            var cultures = options.Value.SupportedCultures;
            foreach (var culture in cultures)
            {
                Product product = productRepository.GetProductWithAll(
                    id + "_" + culture.Name);
                if (product != null)
                {
                    cart.RemoveItem(product, quantity);
                }
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public RedirectToActionResult RemoveAll(
            string productId, string returnUrl)
        {
            var id = productId.Split("_")[0];
            var cultures = options.Value.SupportedCultures;
            foreach (var culture in cultures)
            {
                Product product = productRepository.GetProductWithAll(
                    id + "_" + culture.Name);

                if (product != null)
                {
                    cart.RemoveLine(product);
                }
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public IActionResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
    }
}