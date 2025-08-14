namespace ServerAspNetCoreAPIMakePC.Tests.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using NUnit.Framework;

    using Domain.Entities;
    using Domain.ValueObjects;
    using Infrastructure.Data;
    using Infrastructure.Repositories;

    [TestFixture]
    public class CategoryRepositoryTests
    {
        private MakePCDbContext _context;
        private CategoryRepository _repository;
        private int _catSeq = 1;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MakePCDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._context = new MakePCDbContext(options);
            this._repository = new CategoryRepository(this._context);
        }

        [TearDown]
        public void TearDown()
        {
            this._context.Dispose();
        }

        private Category CreateCategory(int? id = null, string? name = null)
        {
            return new Category
            {
                Id = id ?? _catSeq++,
                Name = new CategoryName(name ?? $"Category{_catSeq}")
            };
        }

        [Test]
        public async Task AddAsync_AddsCategory()
        {
            var category = CreateCategory();

            await this._repository.AddAsync(category);

            var stored = await this._context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            Assert.IsNotNull(stored);
            Assert.AreEqual(category.Name.Value, stored.Name.Value);
        }

        [Test]
        public void AddAsync_Throws_IfCategoryIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this._repository.AddAsync(null));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsCategory_WhenExists()
        {
            var category = CreateCategory();
            this._context.Categories.Add(category);
            await this._context.SaveChangesAsync();

            var found = await this._repository.GetByIdAsync(category.Id);

            Assert.IsNotNull(found);
            Assert.AreEqual(category.Id, found.Id);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var found = await this._repository.GetByIdAsync(-999);
            Assert.IsNull(found);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllCategories()
        {
            this._context.Categories.Add(CreateCategory());
            this._context.Categories.Add(CreateCategory());
            await this._context.SaveChangesAsync();

            var all = (await this._repository.GetAllAsync()).ToList();
            Assert.AreEqual(2, all.Count);
        }

        [Test]
        public async Task UpdateAsync_UpdatesCategory()
        {
            var category = CreateCategory();
            this._context.Categories.Add(category);
            await this._context.SaveChangesAsync();

            category.Name = new CategoryName("UpdatedCategory");
            await this._repository.UpdateAsync(category);

            var updated = await this._context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
            Assert.AreEqual("UpdatedCategory", updated.Name.Value);
        }

        [Test]
        public void UpdateAsync_Throws_IfCategoryIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this._repository.UpdateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_RemovesCategory_WhenExists()
        {
            var category = CreateCategory();
            this._context.Categories.Add(category);
            await this._context.SaveChangesAsync();

            await this._repository.DeleteAsync(category.Id);

            var found = await this._context.Categories.FindAsync(category.Id);
            Assert.IsNull(found);
        }

        [Test]
        public void DeleteAsync_Throws_IfNotFound()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () => this._repository.DeleteAsync(-1234));
        }
    }
}