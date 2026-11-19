using System;
using System.Collections.Generic;
using System.Linq;
using GadgetsOnline.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GadgetsOnline.Services
{
    public class ShoppingCart
    {
        private readonly GadgetsOnlineEntities _store;
        private string ShoppingCartId { get; set; } = string.Empty;

        public const string CartSessionKey = "CartId";

        private ShoppingCart(GadgetsOnlineEntities store)
        {
            _store = store;
        }

        public static ShoppingCart GetCart(HttpContext context, GadgetsOnlineEntities store)
        {
            var cart = new ShoppingCart(store);
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        internal static ShoppingCart CreateForTest(GadgetsOnlineEntities store, string cartId)
        {
            return new ShoppingCart(store) { ShoppingCartId = cartId };
        }

        internal string ShoppingCartId_ForTest => ShoppingCartId;

        internal int CreateOrder(Order order)
        {
            decimal orderTotal = 0;

            var cartItems = GetCartItems();

            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Product.Price,
                    Quantity = item.Count
                };

                orderTotal += item.Count * item.Product.Price;

                _store.OrderDetails.Add(orderDetail);
            }

            order.Total = orderTotal;

            _store.SaveChanges();

            EmptyCart();

            return order.OrderId;
        }

        private void EmptyCart()
        {
            var cartItems = _store.Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                _store.Carts.Remove(cartItem);
            }

            _store.SaveChanges();
        }

        public string GetCartId(HttpContext context)
        {
            if (context.Session.GetString(CartSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity?.Name))
                {
                    context.Session.SetString(CartSessionKey, context.User.Identity.Name);
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    context.Session.SetString(CartSessionKey, tempCartId.ToString());
                }
            }

            return context.Session.GetString(CartSessionKey)!;
        }

        public void AddToCart(int id)
        {
            var cartItem = _store.Carts.SingleOrDefault(
                c => c.CartId == ShoppingCartId && c.ProductId == id);

            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    ProductId = id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                _store.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }

            _store.SaveChanges();
        }

        public int GetCount()
        {
            int? count = (from cartItems in _store.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();

            return count ?? 0;
        }

        internal int RemoveFromCart(int id)
        {
            var cartItem = _store.Carts.Single(
                cart => cart.CartId == ShoppingCartId && cart.ProductId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    _store.Carts.Remove(cartItem);
                }

                _store.SaveChanges();
            }

            return itemCount;
        }

        public List<Cart> GetCartItems()
        {
            return _store.Carts
                .Include(c => c.Product)
                .Where(cart => cart.CartId == ShoppingCartId)
                .ToList();
        }

        public decimal GetTotal()
        {
            decimal? total = (from cartItems in _store.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count * cartItems.Product.Price).Sum();
            return total ?? decimal.Zero;
        }
    }
}
