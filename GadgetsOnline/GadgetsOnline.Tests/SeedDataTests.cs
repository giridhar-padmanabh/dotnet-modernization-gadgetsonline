// Characterization tests for DELTA-008: EF6 seed data → EF Core HasData
// Pin: 5 categories, 13 products, FK assignments match source (GadgetsOnlineDBInitializer.Seed)
using System.Linq;
using GadgetsOnline.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GadgetsOnline.Tests
{
    public class SeedDataTests
    {
        private static GadgetsOnlineEntities CreateInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<GadgetsOnlineEntities>()
                .UseSqlite("Data Source=:memory:")
                .Options;

            var db = new GadgetsOnlineEntities(options);
            db.Database.OpenConnection();
            db.Database.EnsureCreated();
            return db;
        }

        [Fact]
        public void CategoryCount_ShouldBeFive()
        {
            using var db = CreateInMemoryDb();
            Assert.Equal(5, db.Categories.Count());
        }

        [Fact]
        public void ProductCount_ShouldBeThirteen()
        {
            using var db = CreateInMemoryDb();
            Assert.Equal(13, db.Products.Count());
        }

        [Fact]
        public void CategoryNames_MatchSource()
        {
            using var db = CreateInMemoryDb();
            var names = db.Categories.OrderBy(c => c.CategoryId).Select(c => c.Name).ToList();
            Assert.Equal(new[] { "Mobile Phones", "Laptops", "Desktops", "Audio", "Accessories" }, names);
        }

        [Fact]
        public void MobilePhones_HaveThreeProducts()
        {
            using var db = CreateInMemoryDb();
            var count = db.Products.Count(p => p.CategoryId == 1);
            Assert.Equal(3, count);
        }

        [Fact]
        public void Laptops_HaveThreeProducts()
        {
            using var db = CreateInMemoryDb();
            var count = db.Products.Count(p => p.CategoryId == 2);
            Assert.Equal(3, count);
        }

        [Fact]
        public void Desktops_HaveTwoProducts()
        {
            using var db = CreateInMemoryDb();
            var count = db.Products.Count(p => p.CategoryId == 3);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Audio_HaveTwoProducts()
        {
            using var db = CreateInMemoryDb();
            var count = db.Products.Count(p => p.CategoryId == 4);
            Assert.Equal(2, count);
        }

        [Fact]
        public void Accessories_HaveThreeProducts()
        {
            using var db = CreateInMemoryDb();
            var count = db.Products.Count(p => p.CategoryId == 5);
            Assert.Equal(3, count);
        }

        [Fact]
        public void ProductPrices_MatchSource()
        {
            using var db = CreateInMemoryDb();
            var products = db.Products.OrderBy(p => p.ProductId).ToList();

            Assert.Equal(699.00m, products[0].Price);   // Phone 12
            Assert.Equal(999.00m, products[1].Price);   // Phone 13 Pro
            Assert.Equal(1199.00m, products[2].Price);  // Phone 13 Pro Max
            Assert.Equal(899.00m, products[3].Price);   // XTS 13'
            Assert.Equal(479.00m, products[4].Price);   // PC 15.5'
            Assert.Equal(169.00m, products[5].Price);   // Notebook 14
            Assert.Equal(539.00m, products[6].Price);   // The IdeaCenter
            Assert.Equal(389.00m, products[7].Price);   // COMP 22-df003w
            Assert.Equal(28.00m, products[8].Price);    // Bluetooth Headphones
            Assert.Equal(10.00m, products[9].Price);    // ZX Series
            Assert.Equal(9.99m, products[10].Price);    // Wireless charger
            Assert.Equal(2.99m, products[11].Price);    // Mousepad
            Assert.Equal(9.99m, products[12].Price);    // Keyboard
        }
    }
}
