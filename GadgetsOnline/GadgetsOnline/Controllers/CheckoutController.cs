using System;
using System.Threading.Tasks;
using GadgetsOnline.Models;
using GadgetsOnline.Services;
using Microsoft.AspNetCore.Mvc;

namespace GadgetsOnline.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly OrderProcessing _orderProcessing;

        public CheckoutController(OrderProcessing orderProcessing)
        {
            _orderProcessing = orderProcessing;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddressAndPayment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddressAndPayment()
        {
            var order = new Order();
            await TryUpdateModelAsync(order);

            try
            {
                order.Username = "Anonymous";
                order.OrderDate = DateTime.Now;

                _orderProcessing.ProcessOrder(order, this.HttpContext);

                return RedirectToAction("Complete", new { id = order.OrderId });
            }
            catch
            {
                return View(order);
            }
        }

        public IActionResult Complete(int id)
        {
            return View(id);
        }
    }
}
