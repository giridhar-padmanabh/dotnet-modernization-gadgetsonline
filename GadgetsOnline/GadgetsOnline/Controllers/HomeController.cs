using GadgetsOnline.Services;
using Microsoft.AspNetCore.Mvc;

namespace GadgetsOnline.Controllers
{
    public class HomeController : Controller
    {
        private readonly Inventory _inventory;

        public HomeController(Inventory inventory)
        {
            _inventory = inventory;
        }

        public IActionResult Index()
        {
            var products = _inventory.GetBestSellers(6);
            return View(products);
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}
