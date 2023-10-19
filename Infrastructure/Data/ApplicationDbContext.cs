using Core.Domain.Entities;
using Core.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Package> Packages { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Canteen> Canteens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Canteen>().HasData(new Canteen[]
            {
                new Canteen()
                {
                    Id = 1,
                    City = CityEnum.Breda,
                    Location = CanteenEnum.LA,
                    OffersDinners = false
                },
                new Canteen()
                {
                    Id = 2,
                    City = CityEnum.Breda,
                    Location = CanteenEnum.LD,
                    OffersDinners = true
                },
                new Canteen()
                {
                    Id = 3,
                    City = CityEnum.Breda,
                    Location = CanteenEnum.HA,
                    OffersDinners = true
                },
                new Canteen()
                {
                    Id = 4,
                    City = CityEnum.DenBosch,
                    Location = CanteenEnum.OA,
                    OffersDinners = true
                },
                new Canteen()
                {
                    Id = 5,
                    City = CityEnum.Tilburg,
                    Location = CanteenEnum.PCA,
                    OffersDinners = true
                }
            });

            modelBuilder.Entity<Product>().HasData(new Product[]
            {
                new Product()
                {
                    Id = 101,
                    Name = "Unox Sandwich",
                    ContainsAlcohol = false,
                    Picture = "https://www.unileverfoodsolutions.nl/dam/global-ufs/mcos/BENEFRA/calcmenu/recipes/NL-recipes/sandwiches/broodje-unox/main-header.jpg"
                },
                new Product()
                {
                    Id = 502,
                    Name = "Beer",
                    ContainsAlcohol = true,
                    Picture = "https://www.thedrinksbusiness.com/content/uploads/2022/07/iStock-576563556-scaled.jpg"
                },
                new Product()
                {
                    Id = 102,
                    Name = "Chicken Curry Sandwich",
                    ContainsAlcohol = false,
                    Picture = "https://i.pinimg.com/1200x/11/32/2d/11322d34b311fada9ff01551dc5b64ae.jpg"
                },
                new Product()
                {
                    Id = 401,
                    Name = "Pasta Pesto",
                    ContainsAlcohol = false,
                    Picture = "https://recipe-service.prod.cloud.jumbo.com/recipes/1450891-7/Overheerlijke-pasta-pesto_1450891-7-0_560x560"
                },
                new Product()
                {
                    Id = 302,
                    Name = "Tomato Soup",
                    ContainsAlcohol = false,
                    Picture = "https://hips.hearstapps.com/hmg-prod/images/del069923-tomato-soup-131-rv-index-649ded2d7fcd3.jpg"
                },
                new Product()
                {
                    Id = 103,
                    Name = "Grilled Cheese",
                    ContainsAlcohol = false,
                    Picture = "https://cdn.loveandlemons.com/wp-content/uploads/2023/01/grilled-cheese.jpg"
                },
                new Product()
                {
                    Id = 303,
                    Name = "Yogurt",
                    ContainsAlcohol = false,
                    Picture = "https://www.karlijnskitchen.com/wp-content/uploads/2023/04/Yoghurt-maken-in-de-crockpot-express-2.jpg"
                },
                new Product()
                {
                    Id = 402,
                    Name = "Chicken Teriyaki and Rice",
                    ContainsAlcohol = false,
                    Picture = "https://kwokspots.com/wp-content/uploads/2022/10/chicken-teriyaki-high.png"
                },
                new Product()
                {
                    Id = 503,
                    Name = "Wine",
                    ContainsAlcohol = true,
                    Picture = "https://static01.nyt.com/images/2022/02/16/dining/16pour12/16pour12-superJumbo.jpg"
                },
                new Product()
                {
                    Id = 301,
                    Name = "Salad",
                    ContainsAlcohol = false,
                    Picture = "https://www.phood.nl/wp-content/uploads/2015/04/Knapperige-salade-appel-rozijn-9U4.jpg"
                },
                new Product()
                {
                    Id = 501,
                    Name = "Soda",
                    ContainsAlcohol = false,
                    Picture = "https://www.tastingtable.com/img/gallery/17-facts-you-didnt-know-about-soda/l-intro-1680024693.jpg"
                },
                new Product()
                {
                    Id = 202,
                    Name = "French Fries",
                    ContainsAlcohol = false,
                    Picture = "https://img.taste.com.au/ol2Jq8ZQ/taste/2016/11/rachel-87711-2.jpeg"
                },
                new Product()
                {
                    Id = 104,
                    Name = "Cheeseburger",
                    ContainsAlcohol = false,
                    Picture = "https://bettyskitchen.nl/wp-content/uploads/2013/09/het_klassieke_broodje_hamburger_maken_%C2%A9-bettyskitchen_IMG_8426-2.jpg"
                },
                new Product()
                {
                    Id = 201,
                    Name = "Sausage Roll",
                    ContainsAlcohol = false,
                    Picture = "https://www.24kitchen.nl/files/styles/social_media_share/public/2019-11/saucijs.jpeg?itok=LaWWV46s"
                },
                new Product()
                {
                    Id = 203,
                    Name = "Frikandel",
                    ContainsAlcohol = false,
                    Picture = "https://cafetariadekoppel.nl/wp-content/uploads/2020/11/unnamed.jpg"
                }
            });



        }

    }
}
