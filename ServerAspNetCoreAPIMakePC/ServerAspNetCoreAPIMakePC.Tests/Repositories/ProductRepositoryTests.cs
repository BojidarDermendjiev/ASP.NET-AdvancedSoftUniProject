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
    public class ProductRepositoryTests
    {
        private MakePCDbContext _context;
        private ProductRepository _repository;
        private int _catSeq = 1;
        private int _brandSeq = 1;
        private int _productSeq = 1;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MakePCDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            this._context = new MakePCDbContext(options);
            this._repository = new ProductRepository(this._context);
        }

        [TearDown]
        public void TearDown()
        {
            this._context.Dispose();
        }

        private Category CreateCategory()
        {
            return new Category
            {
                Id = _catSeq++,
                Name = new CategoryName("CategoryName")
            };
        }

        private Brand CreateBrand()
        {
            return new Brand
            {
                Id = _brandSeq++,
                Name = new BrandName("Nvidia"),
                Description = "A GPU manufacturer",
                LogoUrl = "https://example.com/nvidia.png",
                Products = new List<Product>()
            };
        }

        private Product CreateProduct(Guid? id = null)
        {
            var productId = id ?? Guid.NewGuid();
            var brand = CreateBrand();
            var category = CreateCategory();
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

        [Test]
        public async Task AddAsync_AddsProduct()
        {
            var product = CreateProduct();
            await this._repository.AddAsync(product);

            var stored = await this._context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            Assert.IsNotNull(stored);
            Assert.AreEqual(product.Name.Value, stored.Name.Value);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsProduct_WhenExists()
        {
            var product = CreateProduct();
            this._context.Products.Add(product);
            await this._context.SaveChangesAsync();

            var found = await this._repository.GetByIdAsync(product.Id);
            Assert.IsNotNull(found);
            Assert.AreEqual(product.Id, found.Id);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
        {
            var found = await this._repository.GetByIdAsync(Guid.NewGuid());
            Assert.IsNull(found);
        }

        [Test]
        public async Task GetByNameAsync_ReturnsProduct_WhenExists()
        {
            var product = CreateProduct();
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var found = await this._repository.GetByNameAsync(product.Name.Value);
            Assert.IsNotNull(found);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllProducts()
        {
            this._context.Products.Add(CreateProduct());
            this._context.Products.Add(CreateProduct());
            await this._context.SaveChangesAsync();

            var all = (await this._repository.GetAllAsync()).ToList();
            Assert.AreEqual(2, all.Count);
        }

        [Test]
        public async Task UpdateAsync_UpdatesProduct()
        {
            var product = CreateProduct();
            this._context.Products.Add(product);
            await this._context.SaveChangesAsync();

            product.Description = "Updated description";
            await this._repository.UpdateAsync(product);

            var updated = await this._context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
            Assert.AreEqual("Updated description", updated.Description);
        }

        [Test]
        public async Task DeleteAsync_RemovesProduct_WhenExists()
        {
            var product = CreateProduct();
            this._context.Products.Add(product);
            await this._context.SaveChangesAsync();

            await this._repository.DeleteAsync(product.Id);

            var found = await this._context.Products.FindAsync(product.Id);
            Assert.IsNull(found);
        }

        [Test]
        public async Task DeleteAsync_DoesNothing_WhenNotExists()
        {
            await this._repository.DeleteAsync(Guid.NewGuid());
            Assert.Pass();
        }

        [Test]
        public void SearchAsync_Throws()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._repository.SearchAsync("query"));
        }

        [Test]
        public async Task GetPagesAsync_ReturnsPaginatedProducts()
        {
            this._context.Products.Add(CreateProduct());
            this._context.Products.Add(CreateProduct());
            this._context.Products.Add(CreateProduct());
            await this._context.SaveChangesAsync();

            var (products, totalCount) = await this._repository.GetPagesAsync(2, 2); 
            Assert.AreEqual(2, totalCount); 
            Assert.IsTrue(products.Count() <= 2);
        }
    }
}