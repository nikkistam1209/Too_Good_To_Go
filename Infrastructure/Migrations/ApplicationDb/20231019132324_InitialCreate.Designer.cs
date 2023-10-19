﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations.ApplicationDb
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231019132324_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Domain.Entities.Canteen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("City")
                        .HasColumnType("int");

                    b.Property<int>("Location")
                        .HasColumnType("int");

                    b.Property<bool>("OffersDinners")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Canteens");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = 1,
                            Location = 0,
                            OffersDinners = false
                        },
                        new
                        {
                            Id = 2,
                            City = 1,
                            Location = 1,
                            OffersDinners = true
                        },
                        new
                        {
                            Id = 3,
                            City = 1,
                            Location = 2,
                            OffersDinners = true
                        },
                        new
                        {
                            Id = 4,
                            City = 2,
                            Location = 3,
                            OffersDinners = true
                        },
                        new
                        {
                            Id = 5,
                            City = 3,
                            Location = 4,
                            OffersDinners = true
                        });
                });

            modelBuilder.Entity("Core.Domain.Entities.Employee", b =>
                {
                    b.Property<string>("EmployeeID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WorkPlace")
                        .HasColumnType("int");

                    b.HasKey("EmployeeID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Core.Domain.Entities.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("AgeRestriction")
                        .HasColumnType("bit");

                    b.Property<int?>("Canteen")
                        .HasColumnType("int");

                    b.Property<DateTime>("ClosingTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PickUp")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("StudentReservationStudentID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StudentReservationStudentID");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("Core.Domain.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("ContainsAlcohol")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Picture")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 101,
                            ContainsAlcohol = false,
                            Name = "Unox Sandwich",
                            Picture = "https://www.unileverfoodsolutions.nl/dam/global-ufs/mcos/BENEFRA/calcmenu/recipes/NL-recipes/sandwiches/broodje-unox/main-header.jpg"
                        },
                        new
                        {
                            Id = 502,
                            ContainsAlcohol = true,
                            Name = "Beer",
                            Picture = "https://www.thedrinksbusiness.com/content/uploads/2022/07/iStock-576563556-scaled.jpg"
                        },
                        new
                        {
                            Id = 102,
                            ContainsAlcohol = false,
                            Name = "Chicken Curry Sandwich",
                            Picture = "https://i.pinimg.com/1200x/11/32/2d/11322d34b311fada9ff01551dc5b64ae.jpg"
                        },
                        new
                        {
                            Id = 401,
                            ContainsAlcohol = false,
                            Name = "Pasta Pesto",
                            Picture = "https://recipe-service.prod.cloud.jumbo.com/recipes/1450891-7/Overheerlijke-pasta-pesto_1450891-7-0_560x560"
                        },
                        new
                        {
                            Id = 302,
                            ContainsAlcohol = false,
                            Name = "Tomato Soup",
                            Picture = "https://hips.hearstapps.com/hmg-prod/images/del069923-tomato-soup-131-rv-index-649ded2d7fcd3.jpg"
                        },
                        new
                        {
                            Id = 103,
                            ContainsAlcohol = false,
                            Name = "Grilled Cheese",
                            Picture = "https://cdn.loveandlemons.com/wp-content/uploads/2023/01/grilled-cheese.jpg"
                        },
                        new
                        {
                            Id = 303,
                            ContainsAlcohol = false,
                            Name = "Yogurt",
                            Picture = "https://www.karlijnskitchen.com/wp-content/uploads/2023/04/Yoghurt-maken-in-de-crockpot-express-2.jpg"
                        },
                        new
                        {
                            Id = 402,
                            ContainsAlcohol = false,
                            Name = "Chicken Teriyaki and Rice",
                            Picture = "https://kwokspots.com/wp-content/uploads/2022/10/chicken-teriyaki-high.png"
                        },
                        new
                        {
                            Id = 503,
                            ContainsAlcohol = true,
                            Name = "Wine",
                            Picture = "https://static01.nyt.com/images/2022/02/16/dining/16pour12/16pour12-superJumbo.jpg"
                        },
                        new
                        {
                            Id = 301,
                            ContainsAlcohol = false,
                            Name = "Salad",
                            Picture = "https://www.phood.nl/wp-content/uploads/2015/04/Knapperige-salade-appel-rozijn-9U4.jpg"
                        },
                        new
                        {
                            Id = 501,
                            ContainsAlcohol = false,
                            Name = "Soda",
                            Picture = "https://www.tastingtable.com/img/gallery/17-facts-you-didnt-know-about-soda/l-intro-1680024693.jpg"
                        },
                        new
                        {
                            Id = 202,
                            ContainsAlcohol = false,
                            Name = "French Fries",
                            Picture = "https://img.taste.com.au/ol2Jq8ZQ/taste/2016/11/rachel-87711-2.jpeg"
                        },
                        new
                        {
                            Id = 104,
                            ContainsAlcohol = false,
                            Name = "Cheeseburger",
                            Picture = "https://bettyskitchen.nl/wp-content/uploads/2013/09/het_klassieke_broodje_hamburger_maken_%C2%A9-bettyskitchen_IMG_8426-2.jpg"
                        },
                        new
                        {
                            Id = 201,
                            ContainsAlcohol = false,
                            Name = "Sausage Roll",
                            Picture = "https://www.24kitchen.nl/files/styles/social_media_share/public/2019-11/saucijs.jpeg?itok=LaWWV46s"
                        },
                        new
                        {
                            Id = 203,
                            ContainsAlcohol = false,
                            Name = "Frikandel",
                            Picture = "https://cafetariadekoppel.nl/wp-content/uploads/2020/11/unnamed.jpg"
                        });
                });

            modelBuilder.Entity("Core.Domain.Entities.Student", b =>
                {
                    b.Property<string>("StudentID")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("City")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DOB")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudentID");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("PackageProduct", b =>
                {
                    b.Property<int>("PackagesId")
                        .HasColumnType("int");

                    b.Property<int>("ProductsId")
                        .HasColumnType("int");

                    b.HasKey("PackagesId", "ProductsId");

                    b.HasIndex("ProductsId");

                    b.ToTable("PackageProduct");
                });

            modelBuilder.Entity("Core.Domain.Entities.Package", b =>
                {
                    b.HasOne("Core.Domain.Entities.Student", "StudentReservation")
                        .WithMany()
                        .HasForeignKey("StudentReservationStudentID");

                    b.Navigation("StudentReservation");
                });

            modelBuilder.Entity("PackageProduct", b =>
                {
                    b.HasOne("Core.Domain.Entities.Package", null)
                        .WithMany()
                        .HasForeignKey("PackagesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Domain.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
