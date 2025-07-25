﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServerAspNetCoreAPIMakePC.Infrastructure.Data;

#nullable disable

namespace ServerAspNetCoreAPIMakePC.Migrations
{
    [DbContext(typeof(MakePCDbContext))]
    [Migration("20250726091727_AddDecimalPrecisionToPrices")]
    partial class AddDecimalPrecisionToPrices
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Basket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Unique identifier for the basket.");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2")
                        .HasComment("The date the basket was created.");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier for the user who owns the basket.");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Baskets");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.BasketItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Unique identifier for the basket item.");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BasketId")
                        .HasColumnType("int")
                        .HasComment("Foreign key referencing the associated basket.");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Foreign key referencing the associated product.");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasComment("The number of units of the product in the basket.");

                    b.Property<Guid?>("ShoppingCartId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("BasketId");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("BasketItems");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Brand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Unique identifier for the brand.");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasComment("Description of the brand.");

                    b.Property<string>("LogoUrl")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)")
                        .HasComment("URL to the brand's logo image.");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Name of the brand.");

                    b.HasKey("Id");

                    b.ToTable("Brands");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Primary key for the Category entity.");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)")
                        .HasComment("Description of the category.");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Name of the category.");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Unique identifier for the order.");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2")
                        .HasComment("The date the order was placed.");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("The payment status of the order.");

                    b.Property<string>("ShippingAddress")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)")
                        .HasComment("The shipping address for the order.");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,4)")
                        .HasComment("The total price of the order.");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier for the user who placed the order.");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.OrderItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Unique identifier for the order item.");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("OrderId")
                        .HasColumnType("int")
                        .HasComment("Foreign key referencing the associated order.");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Foreign key referencing the associated product.");

                    b.Property<int>("Quantity")
                        .HasColumnType("int")
                        .HasComment("The number of units of the product in the order item.");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,4)")
                        .HasComment("The price per unit of the product in the order item.");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.PlatformFeedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Unique identifier for the feedback entry.");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)")
                        .HasComment("Text content of the feedback provided by the user.");

                    b.Property<DateTime>("DateGiven")
                        .HasColumnType("datetime2")
                        .HasComment("The date and time when the feedback was given.");

                    b.Property<int>("Rating")
                        .HasColumnType("int")
                        .HasComment("Identifier for the platform associated with the feedback.");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier for the user who provided the feedback.");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PlatformFeedbacks");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Unique identifier for the product.");

                    b.Property<int>("BrandId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int")
                        .HasComment("Foreign key referencing the category of the product.");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)")
                        .HasComment("Description of the product.");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Image URL of the product.");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Name of the product.");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,4)")
                        .HasComment("Price of the product.");

                    b.Property<string>("Specs")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)")
                        .HasComment("Specifications of the product.");

                    b.Property<int>("Stock")
                        .HasColumnType("int")
                        .HasComment("Stock quantity of the product.");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasComment("Type of the product (e.g., CPU, GPU, etc.).");

                    b.HasKey("Id");

                    b.HasIndex("BrandId");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Unique identifier for the review.");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)")
                        .HasComment("Text content of the review.");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasComment("The date and time when the review was created.");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier for the product being reviewed.");

                    b.Property<int>("Rating")
                        .HasColumnType("int")
                        .HasComment("Rating given in the review, typically on a scale of 1 to 5.");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier for the user who wrote the review.");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.ShoppingCart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Unique identifier for the shopping cart.");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2")
                        .HasComment("Total price of the items in the shopping cart.");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Identifier for the user who owns the shopping cart.");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ShoppingCarts");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasComment("Unique identifier for the user.");

                    b.Property<string>("ConfirmPassword")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Email address of the user.");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Full name of the user.");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)")
                        .HasComment("Password hash for the user.");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)")
                        .HasComment("Role of the user in the system (e.g., Admin, User).");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Basket", b =>
                {
                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.BasketItem", b =>
                {
                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.Basket", "Basket")
                        .WithMany("Items")
                        .HasForeignKey("BasketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.ShoppingCart", null)
                        .WithMany("Items")
                        .HasForeignKey("ShoppingCartId");

                    b.Navigation("Basket");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Order", b =>
                {
                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.OrderItem", b =>
                {
                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.Order", "Order")
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.PlatformFeedback", b =>
                {
                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.User", "User")
                        .WithMany("PlatformFeedbacks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Product", b =>
                {
                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.Brand", "Brand")
                        .WithMany("Products")
                        .HasForeignKey("BrandId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brand");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Review", b =>
                {
                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.Product", "Product")
                        .WithMany("Reviews")
                        .HasForeignKey("ProductId");

                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.User", "User")
                        .WithMany("Reviews")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.ShoppingCart", b =>
                {
                    b.HasOne("ServerAspNetCoreAPIMakePC.Domain.Entities.User", "User")
                        .WithOne("ShoppingCart")
                        .HasForeignKey("ServerAspNetCoreAPIMakePC.Domain.Entities.ShoppingCart", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Basket", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Brand", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Order", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.Product", b =>
                {
                    b.Navigation("Reviews");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.ShoppingCart", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("ServerAspNetCoreAPIMakePC.Domain.Entities.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("PlatformFeedbacks");

                    b.Navigation("Reviews");

                    b.Navigation("ShoppingCart")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
