namespace ServerAspNetCoreAPIMakePC.Tests.Services
{
    using Moq;
    using AutoMapper;
    using NUnit.Framework;
    using Microsoft.Extensions.Options;

    using Domain.Enums;
    using Domain.Entities;
    using Domain.Interfaces;
    using Domain.ValueObjects;
    using Application.DTOs.Product;
    using ServerAspNetCoreAPIMakePC.Application.Services;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Product;

    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductRepository> _productRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private ProductService _productService;

        [SetUp] 
        public void SetUp()
        {
            this._productRepositoryMock = new Mock<IProductRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._productService = new ProductService(this._productRepositoryMock.Object, this._mapperMock.Object);
        }

        [Test]
        public async Task GetProductByIdAsync_ReturnsProduct_IfExists()
        {
            var id = Guid.NewGuid();
            var product = new Product { Id = id };
            this._productRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(product);

            var result = await this._productService.GetProductByIdAsync(id);

            Assert.AreEqual(product, result);
        }

        [Test]
        public async Task GetProductByIdAsync_ReturnsNull_IfNotExists()
        {
            var id = Guid.NewGuid();
            this._productRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product)null);

            var result = await this._productService.GetProductByIdAsync(id);

            Assert.IsNull(result);
        }

        [Test]
        public async Task CreateProductAsync_Throws_IfProductWithSameNameExists()
        {
            var dto = new CreateProductDto { Name = "Laptop" };
            var existing = new Product();

            this._productRepositoryMock.Setup(r => r.GetByNameAsync(dto.Name)).ReturnsAsync(existing);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await this._productService.CreateProductAsync(dto));
            Assert.That(ex.Message, Does.Contain(dto.Name));
        }

        [Test]
        public async Task CreateProductAsync_CreatesProduct_IfNameIsUnique()
        {
            var dto = new CreateProductDto { Name = "Laptop" };
            var product = new Product();

            this._productRepositoryMock.Setup(r => r.GetByNameAsync(dto.Name)).ReturnsAsync((Product)null);
            this._mapperMock.Setup(m => m.Map<Product>(dto)).Returns(product);
            this._productRepositoryMock.Setup(r => r.AddAsync(product)).Returns(Task.CompletedTask);

            var result = await this._productService.CreateProductAsync(dto);

            Assert.IsNotNull(result);
            Assert.AreEqual(product, result);
            this._productRepositoryMock.Verify(r => r.AddAsync(product), Times.Once);
        }

        [Test]
        public async Task UpdateProductAsync_Throws_IfProductNotFound()
        {
            var id = Guid.NewGuid();
            var dto = new UpdateProductDto();

            this._productRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._productService.UpdateProductAsync(id, dto));
        }

        [Test]
        public async Task UpdateProductAsync_Throws_IfProductWithSameNameExists()
        {
            var id = Guid.NewGuid();
            var oldProduct = new Product { Id = id, Name = new ServerAspNetCoreAPIMakePC.Domain.ValueObjects.ProductName("Old") };
            var dto = new UpdateProductDto { Name = "New" };
            var existing = new Product { Id = Guid.NewGuid(), Name = new ServerAspNetCoreAPIMakePC.Domain.ValueObjects.ProductName("New") };

            this._productRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(oldProduct);
            this._productRepositoryMock.Setup(r => r.GetByNameAsync(dto.Name)).ReturnsAsync(existing);

            Assert.ThrowsAsync<InvalidOperationException>(async () => await this._productService.UpdateProductAsync(id, dto));
        }

        [Test]
        public async Task UpdateProductAsync_UpdatesAndReturnsProduct()
        {
            var id = Guid.NewGuid();
            var oldProduct = new Product { Id = id, Name = new ServerAspNetCoreAPIMakePC.Domain.ValueObjects.ProductName("Old") };
            var dto = new UpdateProductDto { Name = "Old" };

            this._productRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(oldProduct);
            this._mapperMock.Setup(m => m.Map(dto, oldProduct));
            this._productRepositoryMock.Setup(r => r.UpdateAsync(oldProduct)).Returns(Task.CompletedTask);

            var result = await this._productService.UpdateProductAsync(id, dto);

            Assert.AreEqual(oldProduct, result);
            this._productRepositoryMock.Verify(r => r.UpdateAsync(oldProduct), Times.Once);
        }

        [Test]
        public async Task DeleteProductAsync_Throws_IfNotFound()
        {
            var id = Guid.NewGuid();
            this._productRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Product)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._productService.DeleteProductAsync(id));
        }       

        [Test]
        public async Task DeleteProductAsync_Deletes_IfFound()
        {
            var id = Guid.NewGuid();
            var product = new Product { Id = id };

            this._productRepositoryMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(product);
            this._productRepositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

            await this._productService.DeleteProductAsync(id);

            this._productRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Test]
        public async Task GetAllProductsAsync_ReturnsAll()
        {
            var products = new List<Product> { new Product(), new Product() };
            this._productRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(products);

            var result = await this.    _productService.GetAllProductsAsync();

            Assert.AreEqual(products, result);
        }

        [Test]
        public async Task ProductExistsAsync_ReturnsTrue_IfExists()
        {
            var name = "Laptop";
            var product = new Product();
            this._productRepositoryMock.Setup(r => r.GetByNameAsync(name)).ReturnsAsync(product);

            var result = await this._productService.ProductExistsAsync(name);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ProductExistsAsync_ReturnsFalse_IfNotExists()
        {
            var name = "Laptop";
            this._productRepositoryMock.Setup(r => r.GetByNameAsync(name)).ReturnsAsync((Product)null);

            var result = await this._productService.ProductExistsAsync(name);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SearchProductsAsync_ReturnsResults()
        {
            var query = "laptop";
            var products = new List<Product> { new Product(), new Product() };
            this._productRepositoryMock.Setup(r => r.SearchAsync(query)).ReturnsAsync(products);

            var result = await this._productService.SearchProductsAsync(query);

            Assert.AreEqual(products, result);
        }

        [Test]
        public async Task GetProductsPagedAsync_ReturnsPagedResults()
        {
            int page = 1, pageSize = 2, total = 5;
            var products = new List<Product> { new Product(), new Product() };
            this._productRepositoryMock.Setup(r => r.GetPagesAsync(page, pageSize)).ReturnsAsync((products, total));

            var (result, count) = await this._productService.GetProductsPagedAsync(page, pageSize);

            Assert.AreEqual(products, result);
            Assert.AreEqual(total, count);
        }
    }
}