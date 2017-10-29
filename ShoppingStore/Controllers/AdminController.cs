using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingStore.Data.Repositories;

namespace ShoppingStore.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository productRepository;
        public AdminController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        public IActionResult Index()
        {
            return View(productRepository.GetProducts());
        }
    }
}