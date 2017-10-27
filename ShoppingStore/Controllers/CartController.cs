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

namespace ShoppingStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository productRepository;
        private IOptions<RequestLocalizationOptions> options;
        private Cart cart;

        public CartController(
            IProductRepository productRepository,
            IOptions<RequestLocalizationOptions> options,
            Cart cartService)
        {
            this.productRepository = productRepository;
            this.options = options;
            cart = cartService;
        }

        [HttpPost]
        public IActionResult AddToCart(
            string productId, string returnUrl)
        {
            var id = productId.Split("_")[0];
            var cultures = options.Value.SupportedCultures;
            foreach (var culture in cultures)
            {
                Product product = productRepository.GetProduct(
                    id + "_" + culture.Name);

                if (product != null)
                {
                    cart.AddItem(product, 1);
                }
            }


            //return RedirectToAction("Index", new { returnUrl });
            return RedirectToAction("Index", "Product");
        }

        public RedirectToActionResult RemoveFromCart(
            string productId, string returnUrl)
        {
            var id = productId.Split("_")[0];
            var cultures = options.Value.SupportedCultures;
            foreach (var culture in cultures)
            {
                Product product = productRepository.GetProduct(
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