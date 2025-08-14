namespace ServerAspNetCoreAPIMakePC.Tests.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    using Domain.Enums;
    using Domain.Entities;
    using Domain.ValueObjects;
    using Infrastructure.Data;
    using Infrastructure.Repositories;

    [TestFixture]
    public class OrderRepositoryTests
    {
        private MakePCDbContext _context;
        private OrderRepository _repository;
        private int _userSeq = 1;
        private int _productSeq = 1;
        private int _orderSeq = 1;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MakePCDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._context = new MakePCDbContext(options);
            this._repository = new OrderRepository(_context);
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

        private OrderItem CreateOrderItem(Product product, Order order, int? id = null, int quantity = 1)
        {
            return new OrderItem
            {
                Id = id ?? 0,
                Order = order,
                ProductId = product.Id,
                Product = product,
                Quantity = new Quantity(quantity),
                UnitPrice = product.Price
            };
        }

        private Order CreateOrder(int? id = null, Guid? userId = null, int itemCount = 1)
        {
            var user = CreateUser(userId);
            this._context.Users.Add(user);
            this._context.SaveChanges();

            var order = new Order
            {
                Id = id ?? _orderSeq++,
                UserId = user.Id,
                User = user,
                OrderDate = DateTime.UtcNow,
                ShippingAddress = new ShippingAddress("Shipping Address 123"),
                PaymentStatus = "Paid",
                TotalPrice = 0m,
                Items = new List<OrderItem>()
            };

            decimal total = 0m;
            for (int i = 0; i < itemCount; i++)
            {
                var product = CreateProduct();
                this._context.Products.Add(product);
                this._context.SaveChanges();
                var item = CreateOrderItem(product, order, i + 1, i + 2);
                order.Items.Add(item);
                total += item.UnitPrice * item.Quantity.Value;
            }
            order.TotalPrice = total;
            return order;
        }

        [Test]
        public async Task AddAsync_AddsOrder()
        {
            var order = CreateOrder(1, null, 2);

            await this._repository.AddAsync(order);

            var stored = await this._context.Orders
                .Include(o => o.Items).ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == order.Id);

            Assert.IsNotNull(stored);
            Assert.AreEqual(2, stored.Items.Count);
            Assert.IsNotNull(stored.User);
            Assert.AreEqual(order.TotalPrice, stored.TotalPrice);
        }

        [Test]
        public void AddAsync_Throws_IfOrderIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _repository.AddAsync(null));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsOrder_WhenExists()
        {
            var order = CreateOrder(2, null, 1);
            this._context.Orders.Add(order);
            await this._context.SaveChangesAsync();

            var found = await this._repository.GetByIdAsync(order.Id);

            Assert.IsNotNull(found);
            Assert.AreEqual(order.Id, found.Id);
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
        public async Task GetByUserIdAsync_ReturnsOrdersForUser()
        {
            var userId = Guid.NewGuid();
            var order1 = CreateOrder(3, userId, 1);
            var order2 = CreateOrder(4, userId, 2);
            _context.Orders.AddRange(order1, order2);
            _context.Orders.Add(CreateOrder(5)); 
            await _context.SaveChangesAsync();

            var orders = (await this._repository.GetByUserIdAsync(userId)).ToList();

            Assert.AreEqual(2, orders.Count);
            Assert.IsTrue(orders.All(o => o.UserId == userId));
            Assert.IsNotEmpty(orders[0].Items);
            Assert.IsNotNull(orders[0].User);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllOrders()
        {
           this. _context.Orders.Add(CreateOrder(6));
            this._context.Orders.Add(CreateOrder(7));
            await this._context.SaveChangesAsync();

            var all = (await this._repository.GetAllAsync()).ToList();
            Assert.AreEqual(2, all.Count);
            Assert.IsNotEmpty(all[0].Items);
            Assert.IsNotNull(all[0].User);
        }

        [Test]
        public async Task UpdateAsync_UpdatesExistingOrder()
        {
            var order = CreateOrder(8, null, 1);
            this._context.Orders.Add(order);
            await this._context.SaveChangesAsync();

            order.PaymentStatus = "Refunded";
            await this._repository.UpdateAsync(order);

            var updated = await this._context.Orders.FirstOrDefaultAsync(o => o.Id == order.Id);
            Assert.AreEqual("Refunded", updated.PaymentStatus);
        }

        [Test]
        public void UpdateAsync_Throws_IfOrderIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this._repository.UpdateAsync(null));
        }

        [Test]
        public async Task UpdateAsync_Throws_IfOrderNotFound()
        {
            var order = CreateOrder(9999);
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._repository.UpdateAsync(order));
        }

        [Test]
        public async Task DeleteAsync_RemovesOrder_WhenExists()
        {
            var order = CreateOrder(10);
            this._context.Orders.Add(order);
            await this._context.SaveChangesAsync();

            await this._repository.DeleteAsync(order.Id);

            var found = await this._context.Orders.FindAsync(order.Id);
            Assert.IsNull(found);
        }

        [Test]
        public void DeleteAsync_Throws_IfOrderNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () => this._repository.DeleteAsync(-1234));
        }
    }
}