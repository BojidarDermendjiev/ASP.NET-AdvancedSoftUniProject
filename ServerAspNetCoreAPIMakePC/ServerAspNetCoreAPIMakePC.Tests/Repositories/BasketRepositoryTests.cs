namespace ServerAspNetCoreAPIMakePC.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;
    
    using Domain.Entities;
    using Domain.ValueObjects;
    using Infrastructure.Data;
    using Infrastructure.Repositories;

    [TestFixture]
    public class BasketRepositoryTests
    {
        private MakePCDbContext _context;
        private BasketRepository _repository;
        private int _userSeq = 1;
        private int _basketSeq = 1;
        private int _productSeq = 1;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MakePCDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._context = new MakePCDbContext(options);
            this._repository = new BasketRepository(this._context);
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
                Role = ServerAspNetCoreAPIMakePC.Domain.Enums.UserRole.User,
                ShoppingCart = new ShoppingCart { UserId = userId }
            };
        }

        private Product CreateProduct(Guid? id = null)
        {
            var productId = id ?? Guid.NewGuid();
            var brand = new Brand
            {
                Id = _productSeq,
                Name = new BrandName("BrandName"),
                Description = "Brand description",
                LogoUrl = "https://example.com/logo.png",
                Products = new List<Product>()
            };
            var category = new Category
            {
                Id = _productSeq,
                Name = new CategoryName("CategoryName")
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
                CategoryId = category.Id,
                Category = category,
                Reviews = new List<Review>()
            };
        }

        private BasketItem CreateBasketItem(Product product, Basket basket, int? id = null, int quantity = 1)
        {
            return new BasketItem
            {
                Id = id ?? 0,
                Basket = basket,
                ProductId = product.Id,
                Product = product,
                Quantity = new Quantity(quantity)
            };
        }

        private Basket CreateBasket(int? id = null, Guid? userId = null, int itemCount = 1)
        {
            var user = CreateUser(userId);
            this._context.Users.Add(user);
            this._context.SaveChanges();

            var basket = new Basket
            {
                Id = id ?? _basketSeq++,
                UserId = user.Id,
                User = user,
                Items = new List<BasketItem>()
            };

            for (int i = 0; i < itemCount; i++)
            {
                var product = CreateProduct();
                this._context.Products.Add(product);
                this._context.SaveChanges();
                var item = CreateBasketItem(product, basket, i + 1, i + 2);
                basket.Items.Add(item);
            }
            return basket;
        }

        [Test]
        public async Task AddAsync_AddsBasket()
        {
            var basket = CreateBasket(1, null, 2);

            await this._repository.AddAsync(basket);

            var stored = await this._context.Baskets
                .Include(b => b.Items).ThenInclude(bi => bi.Product)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == basket.Id);

            Assert.IsNotNull(stored);
            Assert.AreEqual(2, stored.Items.Count);
            Assert.IsNotNull(stored.User);
        }

        [Test]
        public void AddAsync_Throws_IfBasketIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this._repository.AddAsync(null));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsBasket_WhenExists()
        {
            var basket = CreateBasket(2, null, 1);
            this._context.Baskets.Add(basket);
            await this._context.SaveChangesAsync();

            var found = await this._repository.GetByIdAsync(basket.Id);

            Assert.IsNotNull(found);
            Assert.AreEqual(basket.Id, found.Id);
            Assert.IsNotNull(found.User);
            Assert.IsNotEmpty(found.Items);
            Assert.IsNotNull(found.Items.First().Product);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var found = await this._repository.GetByIdAsync(-999);
            Assert.IsNull(found);
        }

        [Test]
        public async Task GetByUserIdAsync_ReturnsBasketForUser()
        {
            var userId = Guid.NewGuid();
            var basket = CreateBasket(3, userId, 2);
            this._context.Baskets.Add(basket);
            this._context.Baskets.Add(CreateBasket(4)); 
            await this._context.SaveChangesAsync();

            var found = await this._repository.GetByUserIdAsync(userId);

            Assert.IsNotNull(found);
            Assert.AreEqual(userId, found.UserId);
            Assert.IsNotEmpty(found.Items);
            Assert.IsNotNull(found.User);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllBaskets()
        {
            this._context.Baskets.Add(CreateBasket(5));
            this._context.Baskets.Add(CreateBasket(6));
            await this._context.SaveChangesAsync();

            var all = (await this._repository.GetAllAsync()).ToList();
            Assert.AreEqual(2, all.Count);
            Assert.IsNotEmpty(all[0].Items);
            Assert.IsNotNull(all[0].User);
        }

        [Test]
        public async Task UpdateAsync_UpdatesExistingBasket()
        {
            var basket = CreateBasket(7, null, 1);
            this._context.Baskets.Add(basket);
            await this._context.SaveChangesAsync();

            var newProduct = CreateProduct();
            this._context.Products.Add(newProduct);
            await this._context.SaveChangesAsync();

            basket.Items.Add(CreateBasketItem(newProduct, basket, 100, 3));
            await this._repository.UpdateAsync(basket);

            var updated = await this._context.Baskets.Include(b => b.Items).FirstOrDefaultAsync(b => b.Id == basket.Id);
            Assert.AreEqual(2, updated.Items.Count);
        }

        [Test]
        public void UpdateAsync_Throws_IfBasketIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this._repository.UpdateAsync(null));
        }

        [Test]
        public async Task UpdateAsync_Throws_IfBasketNotFound()
        {
            var basket = CreateBasket(9999);
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._repository.UpdateAsync(basket));
        }

        [Test]
        public async Task DeleteAsync_RemovesBasket_WhenExists()
        {
            var basket = CreateBasket(8);
            this._context.Baskets.Add(basket);
            await this._context.SaveChangesAsync();

            await this._repository.DeleteAsync(basket.Id);

            var found = await this._context.Baskets.FindAsync(basket.Id);
            Assert.IsNull(found);
        }

        [Test]
        public void DeleteAsync_Throws_IfBasketNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._repository.DeleteAsync(-1234));
        }
    }
}