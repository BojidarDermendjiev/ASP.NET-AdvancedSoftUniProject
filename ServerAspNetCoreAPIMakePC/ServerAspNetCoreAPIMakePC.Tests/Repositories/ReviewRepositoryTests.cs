using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using ServerAspNetCoreAPIMakePC.Domain.Entities;
using ServerAspNetCoreAPIMakePC.Domain.ValueObjects;
using ServerAspNetCoreAPIMakePC.Domain.Enums;
using ServerAspNetCoreAPIMakePC.Infrastructure.Data;
using ServerAspNetCoreAPIMakePC.Infrastructure.Repositories;

namespace ServerAspNetCoreAPIMakePC.Tests.Repositories
{
    [TestFixture]
    public class ReviewRepositoryTests
    {
        private MakePCDbContext _context;
        private Mock<IMapper> _mapperMock;
        private ReviewRepository _repository;
        private int _userSeq = 1;
        private int _productSeq = 1;
        private int _brandSeq = 1;
        private int _categorySeq = 1;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MakePCDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._context = new MakePCDbContext(options);
            this._mapperMock = new Mock<IMapper>();
            this._repository = new ReviewRepository(this._context, this._mapperMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            this._context.Dispose();
        }

        private User CreateUser(Guid? id = null)
        {
            var userId = id ?? Guid.NewGuid();
            return new User
            {
                Id = userId,
                Email = new Email($"user{_userSeq++}@test.com"),
                PasswordHash = "hash",
                ConfirmPassword = "hash",
                PasswordSalt = new byte[] { 1, 2, 3 },
                FullName = new FullName("Test User"),
                Role = UserRole.User,
                ShoppingCart = new ShoppingCart { UserId = userId }
            };
        }

        private Product CreateProduct(Guid? id = null)
        {
            var productId = id ?? Guid.NewGuid();
            var brand = new Brand
            {
                Id = _brandSeq++,
                Name = new BrandName("Nvidia"),
                Description = "A GPU manufacturer",
                LogoUrl = "https://example.com/nvidia.png",
                Products = new List<Product>()
            };
            return new Product
            {
                Id = productId,
                Name = new ProductName($"Product{_productSeq++}"),
                Type = "GPU",
                Brand = brand,
                Price = 99.99m,
                Stock = 10,
                Description = "Some product description",
                Specs = new ProductSpecs("Specs here"),
                ImageUrl = "https://example.com/image.png",
                CategoryId = _categorySeq,
                Category = new Category
                {
                    Id = _categorySeq++,
                    Name = new CategoryName("CategoryName")
                },
                Reviews = new List<Review>()
            };
        }

        private Review CreateReview(int? id = null, Guid? userId = null, Guid? productId = null, string? content = null, int rating = 5)
        {
            var user = CreateUser(userId);
            var product = CreateProduct(productId);
            this._context.Users.Add(user);
            this._context.Products.Add(product);
            this._context.SaveChanges();
            return new Review
            {
                Id = id ?? 0,
                UserId = user.Id,
                User = user,
                ProductId = product.Id,
                Product = product,
                Comment = content ?? "Good product",
                Rating = rating,
                Date = DateTime.UtcNow
            };
        }

        [Test]
        public async Task AddAsync_AddsReviewAndSetsDate()
        {
            var review = CreateReview(1);

            await this._repository.AddAsync(review);

            var stored = await this._context.Reviews.Include(r => r.User).Include(r => r.Product).FirstOrDefaultAsync(r => r.Id == review.Id);
            Assert.IsNotNull(stored);
            Assert.AreEqual(review.Comment, stored.Comment);
            Assert.That((DateTime.UtcNow - stored.Date).TotalSeconds, Is.LessThan(10));
        }

        [Test]
        public void AddAsync_Throws_IfReviewIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this._repository.AddAsync(null));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsReview_WhenExists()
        {
            var review = CreateReview(2);
            this._context.Reviews.Add(review);
            await this._context.SaveChangesAsync();

            var found = await this._repository.GetByIdAsync(review.Id);

            Assert.IsNotNull(found);
            Assert.AreEqual(review.Id, found.Id);
            Assert.IsNotNull(found.User);
            Assert.IsNotNull(found.Product);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var found = await this._repository.GetByIdAsync(-999);
            Assert.IsNull(found);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllReviews()
        {
            this._context.Reviews.Add(CreateReview(3));
            this._context.Reviews.Add(CreateReview(4));
            await this._context.SaveChangesAsync();

            var all = (await this._repository.GetAllAsync()).ToList();
            Assert.AreEqual(2, all.Count);
            Assert.IsNotNull(all[0].User);
            Assert.IsNotNull(all[0].Product);
        }

        [Test]
        public async Task GetByProductIdAsync_ReturnsReviewsForProduct()
        {
            var productId = Guid.NewGuid();
            var review1 = CreateReview(5, null, productId);
            var review2 = CreateReview(6, null, productId);
            this._context.Reviews.AddRange(review1, review2);
            this._context.Reviews.Add(CreateReview(7)); 
            await _context.SaveChangesAsync();

            var reviews = (await this._repository.GetByProductIdAsync(productId)).ToList();

            Assert.AreEqual(2, reviews.Count);
            Assert.IsTrue(reviews.All(r => r.ProductId == productId));
        }

        [Test]
        public async Task GetByUserIdAsync_ReturnsReviewsForUser()
        {
            var userId = Guid.NewGuid();
            var review1 = CreateReview(8, userId);
            var review2 = CreateReview(9, userId);
            this._context.Reviews.AddRange(review1, review2);
            this._context.Reviews.Add(CreateReview(10)); 
            await this._context.SaveChangesAsync();

            var reviews = (await _repository.GetByUserIdAsync(userId)).ToList();

            Assert.AreEqual(2, reviews.Count);
            Assert.IsTrue(reviews.All(r => r.UserId == userId));
        }

        [Test]
        public async Task UpdateAsync_UpdatesExistingReviewAndSetsDate()
        {
            var review = CreateReview(11);
            this._context.Reviews.Add(review);
            await this._context.SaveChangesAsync();

            var updated = CreateReview(11, review.UserId, review.ProductId, "Updated!", 2);

            this._mapperMock.Setup(m => m.Map(updated, It.IsAny<Review>()))
                .Callback<Review, Review>((src, dest) => {
                    dest.Comment = src.Comment;
                    dest.Rating = src.Rating;
                });

            await this._repository.UpdateAsync(updated);

            var stored = await _context.Reviews.FirstOrDefaultAsync(r => r.Id == review.Id);
            Assert.AreEqual("Updated!", stored.Comment);
            Assert.AreEqual(2, stored.Rating);
            Assert.That((DateTime.UtcNow - stored.Date).TotalSeconds, Is.LessThan(10));
        }

        [Test]
        public void UpdateAsync_Throws_IfNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this._repository.UpdateAsync(null));
        }

        [Test]
        public async Task UpdateAsync_Throws_IfNotFound()
        {
            var review = CreateReview(9999);
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._repository.UpdateAsync(review));
        }

        [Test]
        public async Task DeleteAsync_RemovesReview_WhenExists()
        {
            var review = CreateReview(12);
            this._context.Reviews.Add(review);
            await this._context.SaveChangesAsync();

            await this._repository.DeleteAsync(review.Id);

            var found = await this._context.Reviews.FindAsync(review.Id);
            Assert.IsNull(found);
        }

        [Test]
        public void DeleteAsync_Throws_IfNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () => this._repository.DeleteAsync(-1234));
        }
    }
}