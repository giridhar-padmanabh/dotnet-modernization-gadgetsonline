using GadgetsOnline.Models;
using Microsoft.AspNetCore.Http;

namespace GadgetsOnline.Services
{
    public class OrderProcessing
    {
        private readonly GadgetsOnlineEntities _store;

        public OrderProcessing(GadgetsOnlineEntities store)
        {
            _store = store;
        }

        internal bool ProcessOrder(Order order, HttpContext httpContext)
        {
            _store.Orders.Add(order);
            _store.SaveChanges();

            var cart = ShoppingCart.GetCart(httpContext, _store);
            cart.CreateOrder(order);

            return true;
        }
    }
}
