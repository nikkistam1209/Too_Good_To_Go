using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations.ApplicationDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Canteens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<int>(type: "int", nullable: false),
                    OffersDinners = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canteens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkPlace = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContainsAlcohol = table.Column<bool>(type: "bit", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<int>(type: "int", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.StudentID);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Canteen = table.Column<int>(type: "int", nullable: true),
                    PickUp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AgeRestriction = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    StudentReservationStudentID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Packages_Students_StudentReservationStudentID",
                        column: x => x.StudentReservationStudentID,
                        principalTable: "Students",
                        principalColumn: "StudentID");
                });

            migrationBuilder.CreateTable(
                name: "PackageProduct",
                columns: table => new
                {
                    PackagesId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageProduct", x => new { x.PackagesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_PackageProduct_Packages_PackagesId",
                        column: x => x.PackagesId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Canteens",
                columns: new[] { "Id", "City", "Location", "OffersDinners" },
                values: new object[,]
                {
                    { 1, 1, 0, false },
                    { 2, 1, 1, true },
                    { 3, 1, 2, true },
                    { 4, 2, 3, true },
                    { 5, 3, 4, true }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ContainsAlcohol", "Name", "Picture" },
                values: new object[,]
                {
                    { 101, false, "Unox Sandwich", "https://www.unileverfoodsolutions.nl/dam/global-ufs/mcos/BENEFRA/calcmenu/recipes/NL-recipes/sandwiches/broodje-unox/main-header.jpg" },
                    { 102, false, "Chicken Curry Sandwich", "https://i.pinimg.com/1200x/11/32/2d/11322d34b311fada9ff01551dc5b64ae.jpg" },
                    { 103, false, "Grilled Cheese", "https://cdn.loveandlemons.com/wp-content/uploads/2023/01/grilled-cheese.jpg" },
                    { 104, false, "Cheeseburger", "https://bettyskitchen.nl/wp-content/uploads/2013/09/het_klassieke_broodje_hamburger_maken_%C2%A9-bettyskitchen_IMG_8426-2.jpg" },
                    { 201, false, "Sausage Roll", "https://www.24kitchen.nl/files/styles/social_media_share/public/2019-11/saucijs.jpeg?itok=LaWWV46s" },
                    { 202, false, "French Fries", "https://img.taste.com.au/ol2Jq8ZQ/taste/2016/11/rachel-87711-2.jpeg" },
                    { 203, false, "Frikandel", "https://cafetariadekoppel.nl/wp-content/uploads/2020/11/unnamed.jpg" },
                    { 301, false, "Salad", "https://www.phood.nl/wp-content/uploads/2015/04/Knapperige-salade-appel-rozijn-9U4.jpg" },
                    { 302, false, "Tomato Soup", "https://hips.hearstapps.com/hmg-prod/images/del069923-tomato-soup-131-rv-index-649ded2d7fcd3.jpg" },
                    { 303, false, "Yogurt", "https://www.karlijnskitchen.com/wp-content/uploads/2023/04/Yoghurt-maken-in-de-crockpot-express-2.jpg" },
                    { 401, false, "Pasta Pesto", "https://recipe-service.prod.cloud.jumbo.com/recipes/1450891-7/Overheerlijke-pasta-pesto_1450891-7-0_560x560" },
                    { 402, false, "Chicken Teriyaki and Rice", "https://kwokspots.com/wp-content/uploads/2022/10/chicken-teriyaki-high.png" },
                    { 501, false, "Soda", "https://www.tastingtable.com/img/gallery/17-facts-you-didnt-know-about-soda/l-intro-1680024693.jpg" },
                    { 502, true, "Beer", "https://www.thedrinksbusiness.com/content/uploads/2022/07/iStock-576563556-scaled.jpg" },
                    { 503, true, "Wine", "https://static01.nyt.com/images/2022/02/16/dining/16pour12/16pour12-superJumbo.jpg" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageProduct_ProductsId",
                table: "PackageProduct",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_StudentReservationStudentID",
                table: "Packages",
                column: "StudentReservationStudentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Canteens");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "PackageProduct");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
