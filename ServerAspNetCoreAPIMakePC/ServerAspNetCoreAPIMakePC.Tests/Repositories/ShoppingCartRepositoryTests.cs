namespace ServerAspNetCoreAPIMakePC.Tests.Repositories
{
    using AutoMapper;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using NUnit.Framework;

    using Domain.Entities;
    using Domain.ValueObjects;
    using Infrastructure.Data;
    using Infrastructure.Repositories;

    [TestFixture]
    public class ShoppingCartRepositoryTests
    {
        private MakePCDbContext _context;
        private Mock<IMapper> _mapperMock;
        private ShoppingCartRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MakePCDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._context = new MakePCDbContext(options);
            this._mapperMock = new Mock<IMapper>();
            this._repository = new ShoppingCartRepository(this._context, this._mapperMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            this._context.Dispose();
        }

        private ShoppingCart CreateCart(Guid userId, params BasketItem[] items)
        {
            return new ShoppingCart
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                DateCreated = DateTime.UtcNow,
                Items = items.ToList()
            };
        }

        private BasketItem CreateItem(int? id = null, int quantity = 1)
        {
            return new BasketItem
            {
                Id = id ?? 0,
                BasketId = 0,
                Basket = null!,
                ProductId = Guid.NewGuid(),
                Product = null!,
                Quantity = new Quantity(quantity)
            };
        }

        [Test]
        public async Task GetByUserIdAsync_ReturnsCart_WhenExists()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId, CreateItem(), CreateItem());
            this._context.ShoppingCarts.Add(cart);
            await this._context.SaveChangesAsync();

            var found = await this._repository.GetByUserIdAsync(userId);

            Assert.IsNotNull(found);
            Assert.AreEqual(userId, found.UserId);
            Assert.AreEqual(2, found.Items.Count);
        }

        [Test]
        public async Task GetByUserIdAsync_ReturnsNull_WhenNotExists()
        {
            var found = await this._repository.GetByUserIdAsync(Guid.NewGuid());
            Assert.IsNull(found);
        }

        [Test]
        public async Task UpsertAsync_AddsNewCart_WhenNotExists()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId, CreateItem(), CreateItem());

            await this._repository.UpsertAsync(cart);

            var stored = await this._context.ShoppingCarts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
            Assert.IsNotNull(stored);
            Assert.AreEqual(2, stored.Items.Count);
        }

        [Test]
        public async Task UpsertAsync_UpdatesExistingCart_AddsAndRemovesItems()
        {
            var userId = Guid.NewGuid();
            var existingItem = CreateItem(1, 1); 
            var cart = CreateCart(userId, existingItem);
            this._context.ShoppingCarts.Add(cart);
            await this._context.SaveChangesAsync();

            var newItem = CreateItem(2, 5); 
            var updatedCart = CreateCart(userId, newItem);

            this._mapperMock.Setup(m => m.Map(It.IsAny<BasketItem>(), It.IsAny<BasketItem>()));

            await this._repository.UpsertAsync(updatedCart);

            var stored = await this._context.ShoppingCarts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
            Assert.IsNotNull(stored);
            Assert.AreEqual(1, stored.Items.Count);
            Assert.AreEqual(newItem.Id, stored.Items.First().Id);
        }

        [Test]
        public async Task UpsertAsync_UpdatesExistingCart_MapsExistingItems()
        {
            var userId = Guid.NewGuid();
            var item = CreateItem(1, 1);
            var cart = CreateCart(userId, item);
            this._context.ShoppingCarts.Add(cart);
            await this._context.SaveChangesAsync();

            var updatedItem = CreateItem(1, 10); 
            var updatedCart = CreateCart(userId, updatedItem);

            this._mapperMock.Setup(m => m.Map(updatedItem, It.IsAny<BasketItem>()))
                .Callback<BasketItem, BasketItem>((src, dest) => dest.Quantity = src.Quantity);

            await this._repository.UpsertAsync(updatedCart);

            var stored = await this._context.ShoppingCarts.Include(c => c.Items).FirstOrDefaultAsync(c => c.UserId == userId);
            Assert.IsNotNull(stored);
            Assert.AreEqual(1, stored.Items.Count);
            Assert.AreEqual(10, stored.Items.First().Quantity.Value); 
        }

        [Test]
        public async Task DeleteByUserIdAsync_RemovesCart_IfExists()
        {
            var userId = Guid.NewGuid();
            var cart = CreateCart(userId, CreateItem());
            this._context.ShoppingCarts.Add(cart);
            await this._context.SaveChangesAsync();

            await this._repository.DeleteByUserIdAsync(userId);

            var found = await this._context.ShoppingCarts.FirstOrDefaultAsync(c => c.UserId == userId);
            Assert.IsNull(found);
        }

        [Test]
        public async Task DeleteByUserIdAsync_DoesNothing_IfNotExists()
        {
            await this._repository.DeleteByUserIdAsync(Guid.NewGuid());
            Assert.Pass();
        }
    }
}