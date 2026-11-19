using System.Collections.Generic;
using GadgetsOnline.Models;
using GadgetsOnline.Services;
using Microsoft.AspNetCore.Mvc;

namespace GadgetsOnline.Controllers
{
    public class StoreController : Controller
    {
        private readonly Inventory _inventory;

        public StoreController(Inventory inventory)
        {
            _inventory = inventory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Browse(string category)
        {
            var productModel = _inventory.GetAllProductsInCategory(category);
            return View(productModel);
        }

        public IActionResult Details(int id)
        {
            var product = _inventory.GetProductById(id);
            return View(product);
        }
    }
}
