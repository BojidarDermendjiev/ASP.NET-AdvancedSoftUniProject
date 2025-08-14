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
    public class BrandRepositoryTests
    {
        private MakePCDbContext _context;
        private BrandRepository _repository;
        private int _brandSeq = 1;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MakePCDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._context = new MakePCDbContext(options);
            this._repository = new BrandRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            this._context.Dispose();
        }

        private Brand CreateBrand(int? id = null, string? name = null)
        {
            return new Brand
            {
                Id = id ?? _brandSeq++,
                Name = new BrandName(name ?? $"Brand{_brandSeq}"),
                Description = "Brand description",
                LogoUrl = $"https://example.com/logo{_brandSeq}.png",
                Products = new List<Product>()
            };
        }

        [Test]
        public async Task AddAsync_AddsBrand()
        {
            var brand = CreateBrand();

            await this._repository.AddAsync(brand);

            var stored = await this._context.Brands.FirstOrDefaultAsync(b => b.Id == brand.Id);
            Assert.IsNotNull(stored);
            Assert.AreEqual(brand.Name.Value, stored.Name.Value);
        }

        [Test]
        public void AddAsync_Throws_IfBrandIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this._repository.AddAsync(null));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsBrand_WhenExists()
        {
            var brand = CreateBrand();
            this._context.Brands.Add(brand);
            await this._context.SaveChangesAsync();

            var found = await this._repository.GetByIdAsync(brand.Id);

            Assert.IsNotNull(found);
            Assert.AreEqual(brand.Id, found.Id);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var found = await this._repository.GetByIdAsync(-999);
            Assert.IsNull(found);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllBrands()
        {
            this._context.Brands.Add(CreateBrand());
            this._context.Brands.Add(CreateBrand());
            await this._context.SaveChangesAsync();

            var all = (await this._repository.GetAllAsync()).ToList();
            Assert.AreEqual(2, all.Count);
        }

        [Test]
        public async Task UpdateAsync_UpdatesBrand()
        {
            var brand = CreateBrand();
            this._context.Brands.Add(brand);
            await this._context.SaveChangesAsync();

            brand.Name = new BrandName("UpdatedBrand");
            await this._repository.UpdateAsync(brand);

            var updated = await this._context.Brands.FirstOrDefaultAsync(b => b.Id == brand.Id);
            Assert.AreEqual("UpdatedBrand", updated.Name.Value);
        }

        [Test]
        public void UpdateAsync_Throws_IfBrandIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await this._repository.UpdateAsync(null));
        }

        [Test]
        public async Task DeleteAsync_RemovesBrand_WhenExists()
        {
            var brand = CreateBrand();
            this._context.Brands.Add(brand);
            await this._context.SaveChangesAsync();

            await this._repository.DeleteAsync(brand.Id);

            var found = await this._context.Brands.FindAsync(brand.Id);
            Assert.IsNull(found);
        }

        [Test]
        public void DeleteAsync_Throws_IfNotFound()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => this._repository.DeleteAsync(-1234));
        }
    }
}