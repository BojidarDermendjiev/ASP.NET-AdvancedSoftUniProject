namespace ServerAspNetCoreAPIMakePC.Tests.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    using Moq;
    using NUnit.Framework;

    using API.Controllers;
    using Domain.Entities;
    using Application.Interfaces;
    using Application.DTOs.Product;

    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductService> _productServiceMock;
        private ProductController _controller;

        [SetUp]
        public void SetUp()
        {
            this._productServiceMock = new Mock<IProductService>();
            this._controller = new ProductController(this._productServiceMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkWithProducts()
        {
            var products = new List<Product> { new Product { Id = Guid.NewGuid() } };
            this._productServiceMock.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(products);

            var result = await this._controller.GetAll();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreSame(products, okResult.Value);
        }

        [Test]
        public async Task GetById_ProductExists_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var product = new Product { Id = id };
            this._productServiceMock.Setup(s => s.GetProductByIdAsync(id)).ReturnsAsync(product);

            var result = await this._controller.GetById(id);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreSame(product, okResult.Value);
        }

        [Test]
        public async Task GetById_ProductNotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            this._productServiceMock.Setup(s => s.GetProductByIdAsync(id)).ReturnsAsync((Product)null);

            var result = await this._controller.GetById(id);

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task Create_ProductCreated_ReturnsCreatedAtAction()
        {
            var dto = new CreateProductDto { Name = "Test" };
            var product = new Product { Id = Guid.NewGuid() };
            this._productServiceMock.Setup(s => s.CreateProductAsync(dto)).ReturnsAsync(product);

            var result = await this._controller.Create(dto);

            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(nameof(this._controller.GetById), createdResult.ActionName);
            Assert.AreEqual(product, createdResult.Value);
        }

        [Test]
        public async Task Create_ThrowsException_ReturnsConflict()
        {
            var dto = new CreateProductDto { Name = "Test" };
            this._productServiceMock.Setup(s => s.CreateProductAsync(dto)).ThrowsAsync(new Exception("Error"));

            var result = await this._controller.Create(dto);

            var conflict = result.Result as ConflictObjectResult;
            Assert.IsNotNull(conflict);
            Assert.AreEqual(409, conflict.StatusCode);
        }

        [Test]
        public async Task Update_ProductUpdated_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var dto = new UpdateProductDto { Name = "Updated" };
            var product = new Product { Id = id };
            this._productServiceMock.Setup(s => s.UpdateProductAsync(id, dto)).ReturnsAsync(product);

            var result = await _controller.Update(id, dto);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(product, okResult.Value);
        }

        [Test]
        public async Task Update_ProductNotFound_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            var dto = new UpdateProductDto { Name = "Updated" };
            this._productServiceMock.Setup(s => s.UpdateProductAsync(id, dto)).ReturnsAsync((Product)null);

            var result = await _controller.Update(id, dto);

            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task Update_ThrowsKeyNotFoundException_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            var dto = new UpdateProductDto { Name = "Updated" };
            this._productServiceMock.Setup(s => s.UpdateProductAsync(id, dto)).ThrowsAsync(new KeyNotFoundException("Not found"));

            var result = await _controller.Update(id, dto);

            var notFound = result.Result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
        }

        [Test]
        public async Task Update_ThrowsInvalidOperationException_ReturnsConflict()
        {
            var id = Guid.NewGuid();
            var dto = new UpdateProductDto { Name = "Updated" };
            this._productServiceMock.Setup(s => s.UpdateProductAsync(id, dto)).ThrowsAsync(new InvalidOperationException("Conflict"));

            var result = await _controller.Update(id, dto);

            var conflict = result.Result as ConflictObjectResult;
            Assert.IsNotNull(conflict);
            Assert.AreEqual(409, conflict.StatusCode);
        }

        [Test]
        public async Task Delete_Success_ReturnsNoContent()
        {
            var id = Guid.NewGuid();
            this._productServiceMock.Setup(s => s.DeleteProductAsync(id)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(id);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Delete_ThrowsKeyNotFoundException_ReturnsNotFound()
        {
            var id = Guid.NewGuid();
            this._productServiceMock.Setup(s => s.DeleteProductAsync(id)).ThrowsAsync(new KeyNotFoundException("Not found"));

            var result = await _controller.Delete(id);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.AreEqual(404, notFound.StatusCode);
        }

        [Test]
        public async Task Search_ReturnsOkWithResults()
        {
            var query = "laptop";
            var products = new List<Product> { new Product { Id = Guid.NewGuid() } };
            this._productServiceMock.Setup(s => s.SearchProductsAsync(query)).ReturnsAsync(products);

            var result = await _controller.Search(query);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreSame(products, okResult.Value);
        }

        [Test]
        public async Task GetPaged_ReturnsOkWithResponse()
        {
            var products = new List<Product> { new Product { Id = Guid.NewGuid() } };
            int totalCount = 11;

            this._productServiceMock.Setup(s => s.GetProductsPagedAsync(1, 10)).ReturnsAsync((products, totalCount));

            var result = await _controller.GetPaged(1, 10);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            dynamic response = okResult.Value;
            Assert.AreEqual(1, response.Page);
            Assert.AreEqual(10, response.PageSize);
            Assert.AreEqual(11, response.TotalCount);
            Assert.AreEqual(2, response.TotalPages);
        }

        [Test]
        public async Task GetPaged_InvalidPageOrSize_ReturnsBadRequest()
        {
            var result1 = await this._controller.GetPaged(0, 10);
            var result2 = await this._controller.GetPaged(1, 0);

            Assert.IsInstanceOf<BadRequestObjectResult>(result1);
            Assert.IsInstanceOf<BadRequestObjectResult>(result2);
        }
    }
}

