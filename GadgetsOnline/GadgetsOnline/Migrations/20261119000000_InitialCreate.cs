using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace GadgetsOnline.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 160, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 70, nullable: false),
                    City = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    State = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    PostalCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 24, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    ProductArtUrl = table.Column<string>(type: "TEXT", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    RecordId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CartId = table.Column<string>(type: "TEXT", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.RecordId);
                    table.ForeignKey(
                        name: "FK_Carts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Latest collection of Mobile Phones", "Mobile Phones" },
                    { 2, "Latest Laptops in 2022", "Laptops" },
                    { 3, "Latest Desktops in 2022", "Desktops" },
                    { 4, "Latest audio devices", "Audio" },
                    { 5, "USB Cables, Mobile chargers and Keyboards etc", "Accessories" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "CategoryId", "Name", "Price", "ProductArtUrl" },
                values: new object[,]
                {
                    { 1,  1, "Phone 12",                      699.00m,  "/Content/Images/Mobile/1.jpg" },
                    { 2,  1, "Phone 13 Pro",                  999.00m,  "/Content/Images/Mobile/2.jpg" },
                    { 3,  1, "Phone 13 Pro Max",              1199.00m, "/Content/Images/Mobile/3.jpg" },
                    { 4,  2, "XTS 13'",                       899.00m,  "/Content/Images/Laptop/1.jpg" },
                    { 5,  2, "PC 15.5'",                      479.00m,  "/Content/Images/Laptop/2.jpg" },
                    { 6,  2, "Notebook 14",                   169.00m,  "/Content/Images/Laptop/3.jpg" },
                    { 7,  3, "The IdeaCenter",                539.00m,  "/Content/Images/placeholder.gif" },
                    { 8,  3, "COMP 22-df003w",                389.00m,  "/Content/Images/placeholder.gif" },
                    { 9,  4, "Bluetooth Headphones Over Ear", 28.00m,   "/Content/Images/Headphones/1.png" },
                    { 10, 4, "ZX Series ",                    10.00m,   "/Content/Images/Headphones/2.png" },
                    { 11, 5, "Wireless charger",              9.99m,    "/Content/Images/placeholder.gif" },
                    { 12, 5, "Mousepad",                      2.99m,    "/Content/Images/placeholder.gif" },
                    { 13, 5, "Keyboard",                      9.99m,    "/Content/Images/placeholder.gif" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductId",
                table: "Carts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Carts");
            migrationBuilder.DropTable(name: "OrderDetails");
            migrationBuilder.DropTable(name: "Products");
            migrationBuilder.DropTable(name: "Orders");
            migrationBuilder.DropTable(name: "Categories");
        }
    }
}
