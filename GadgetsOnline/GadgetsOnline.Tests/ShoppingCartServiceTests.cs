// Characterization tests for DELTA-027 (DbContext DI lifetime) and DELTA-009 (session)
// Pin: cart operations use injected scoped DbContext; cart totals calculated correctly
using System;
using System.Linq;
using GadgetsOnline.Models;
using GadgetsOnline.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GadgetsOnline.Tests
{
    public class ShoppingCartServiceTests
    {
        private static GadgetsOnlineEntities CreateDb()
        {
            var options = new DbContextOptionsBuilder<GadgetsOnlineEntities>()
                .UseSqlite("Data Source=:memory:")
                .Options;
            var db = new GadgetsOnlineEntities(options);
            db.Database.OpenConnection();
            db.Database.EnsureCreated();
            return db;
        }

        private static ShoppingCart CreateCartWithId(GadgetsOnlineEntities db, string cartId)
        {
            // Use reflection to set ShoppingCartId since GetCart needs HttpContext
            var cart = new ShoppingCart_TestAccessor(db, cartId);
            return cart.Cart;
        }

        [Fact]
        public void AddToCart_IncreasesCount()
        {
            using var db = CreateDb();
            var cart = new CartTestHelper(db, "test-cart-001");

            cart.AddToCart(1);
            cart.AddToCart(1);

            Assert.Equal(2, cart.GetCount());
        }

        [Fact]
        public void GetTotal_ReturnsCorrectSum()
        {
            using var db = CreateDb();
            var cart = new CartTestHelper(db, "test-cart-002");

            // Phone 12 (ProductId=1) costs $699.00; add 2 units
            cart.AddToCart(1);
            cart.AddToCart(1);

            Assert.Equal(1398.00m, cart.GetTotal());
        }

        [Fact]
        public void RemoveFromCart_DecreasesCount()
        {
            using var db = CreateDb();
            var cart = new CartTestHelper(db, "test-cart-003");

            cart.AddToCart(1);
            cart.AddToCart(1);
            cart.RemoveFromCart(1);

            Assert.Equal(1, cart.GetCount());
        }

        [Fact]
        public void GetCartItems_ReturnsItemsWithProductLoaded()
        {
            using var db = CreateDb();
            var cart = new CartTestHelper(db, "test-cart-004");

            cart.AddToCart(1); // Phone 12

            var items = cart.GetCartItems();
            Assert.Single(items);
            Assert.NotNull(items[0].Product);
            Assert.Equal("Phone 12", items[0].Product.Name);
        }

        [Fact]
        public void GetCartId_AnonymousUser_ReturnsGuid()
        {
            using var db = CreateDb();

            var context = new DefaultHttpContext();
            context.Session = new TestSession();

            // ShoppingCart uses GetCartId to assign a GUID for anonymous users
            var cart = ShoppingCart.GetCart(context, db);
            var cartId = cart.ShoppingCartId_ForTest;

            Assert.False(string.IsNullOrEmpty(cartId));
            Assert.True(Guid.TryParse(cartId, out _), "Expected GUID cart ID for anonymous user");
        }

        [Fact]
        public void GetCartId_SameSession_ReturnsSameId()
        {
            using var db = CreateDb();

            var session = new TestSession();
            var context1 = new DefaultHttpContext { Session = session };
            var context2 = new DefaultHttpContext { Session = session };

            var cart1 = ShoppingCart.GetCart(context1, db);
            var id1 = cart1.ShoppingCartId_ForTest;

            var cart2 = ShoppingCart.GetCart(context2, db);
            var id2 = cart2.ShoppingCartId_ForTest;

            Assert.Equal(id1, id2);
        }
    }

    // Test helper: wraps ShoppingCart with direct constructor access for testing
    file class CartTestHelper
    {
        private readonly ShoppingCart _inner;

        public CartTestHelper(GadgetsOnlineEntities db, string cartId)
        {
            _inner = ShoppingCart.CreateForTest(db, cartId);
        }

        public void AddToCart(int productId) => _inner.AddToCart(productId);
        public int GetCount() => _inner.GetCount();
        public decimal GetTotal() => _inner.GetTotal();
        public int RemoveFromCart(int productId) => _inner.RemoveFromCart(productId);
        public System.Collections.Generic.List<Cart> GetCartItems() => _inner.GetCartItems();
    }

    // Minimal in-memory ISession for testing session ID assignment
    file class TestSession : ISession
    {
        private readonly System.Collections.Generic.Dictionary<string, byte[]> _store = new();

        public bool IsAvailable => true;
        public string Id => "test-session";
        public System.Collections.Generic.IEnumerable<string> Keys => _store.Keys;

        public void Clear() => _store.Clear();
        public System.Threading.Tasks.Task CommitAsync(System.Threading.CancellationToken cancellationToken = default) => System.Threading.Tasks.Task.CompletedTask;
        public System.Threading.Tasks.Task LoadAsync(System.Threading.CancellationToken cancellationToken = default) => System.Threading.Tasks.Task.CompletedTask;
        public void Remove(string key) => _store.Remove(key);
        public void Set(string key, byte[] value) => _store[key] = value;
        public bool TryGetValue(string key, out byte[] value) => _store.TryGetValue(key, out value!);
    }
}
