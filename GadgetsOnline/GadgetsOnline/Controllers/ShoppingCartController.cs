using System.Net;
using GadgetsOnline.Models;
using GadgetsOnline.Services;
using GadgetsOnline.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GadgetsOnline.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly GadgetsOnlineEntities _store;
        private readonly Inventory _inventory;

        public ShoppingCartController(GadgetsOnlineEntities store, Inventory inventory)
        {
            _store = store;
            _inventory = inventory;
        }

        public IActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext, _store);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }

        public IActionResult AddToCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext, _store);
            cart.AddToCart(id);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext, _store);
            int itemCount = cart.RemoveFromCart(id);
            var productName = _inventory.GetProductNameById(id);

            var results = new ShoppingCartRemoveViewModel
            {
                Message = WebUtility.HtmlEncode(productName) + " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return RedirectToAction("Index");
        }
    }
}
