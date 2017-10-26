using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingStore.Data.Repositories;
using ShoppingStore.Models;
using ShoppingStore.Infrastructures.Extensions;
using ShoppingStore.Models.CartViewModels;

namespace ShoppingStore.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository productRepository;
        private Cart cart;

        public CartController(
            IProductRepository productRepository,
            Cart cartService)
        {
            this.productRepository = productRepository;
            cart = cartService;
        }

        [HttpPost]
        public RedirectToActionResult AddToCart(
            string productId, string returnUrl)
        {
            Product product = productRepository.GetProduct(productId);
            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(
            string productId, string returnUrl)
        {
            Product product = productRepository.GetProduct(productId);

            if (product != null)
            {
                cart.RemoveLine(product);
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