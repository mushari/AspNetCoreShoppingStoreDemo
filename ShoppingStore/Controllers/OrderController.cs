using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingStore.Models;
using ShoppingStore.Data.Repositories;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ShoppingStore.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private IOrderRepository orderRepository;
        private UserManager<ApplicationUser> userManager;
        private Cart cart;
        public OrderController(
            IOrderRepository orderRepository,
            UserManager<ApplicationUser> userManager,
            Cart cartService)
        {
            this.orderRepository = orderRepository;
            this.userManager = userManager;
            this.cart = cartService;
        }

        public IActionResult Checkout()
        {
            return View(new Order());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User);
            var currentCulture = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;
            var cartLines = cart.GetCartLines.Where(
                l => l.Product.ProductId.EndsWith("_" + currentCulture) &&
                l.User.Id == currentUser.Id);

            if (cartLines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry your cart is empty");
            }

            if (ModelState.IsValid)
            {
                order.Lines = cartLines.ToArray();
                if (order.Lines != null)
                {
                    orderRepository.SaveOrder(order);
                }

                return RedirectToAction(nameof(Completed));
            }
            else
            {
                return View(order);
            }
        }

        public IActionResult List()
        {
            return View(orderRepository.Orders.Where(o => !o.Shipped));
        }

        [HttpPost]
        public IActionResult MarkShipped(int orderId)
        {
            Order order = orderRepository.Orders.SingleOrDefault(o => o.OrderId == orderId);
            if (order != null)
            {
                order.Shipped = true;
                orderRepository.SaveOrder(order);
            }

            return RedirectToAction(nameof(List));
        }

        public ViewResult Completed()
        {
            cart.Clear();
            return View();
        }
    }
}