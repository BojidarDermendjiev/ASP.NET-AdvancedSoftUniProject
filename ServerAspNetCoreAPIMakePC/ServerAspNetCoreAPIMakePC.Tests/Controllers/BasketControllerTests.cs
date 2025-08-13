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
    using Application.DTOs.Basket;

    [TestFixture]
    public class BasketControllerTests
    {
        private Mock<IBasketService> _basketServiceMock;
        private BasketController _controller;

        private const string BasketMismatch = "Basket ID mismatch.";

        [SetUp]
        public void SetUp()
        {
            this._basketServiceMock = new Mock<IBasketService>();
            this._controller = new BasketController(this._basketServiceMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkWithBaskets()
        {
            var baskets = new List<BasketDto> { new BasketDto { Id = 1 } };
            this._basketServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(baskets);

            var result = await this._controller.GetAll();

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(baskets, ok.Value);
        }

        [Test]
        public async Task GetById_Found_ReturnsOk()
        {
            var basket = new BasketDto { Id = 1 };
            this._basketServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(basket);

            var result = await this._controller.GetById(1);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(basket, ok.Value);
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            this._basketServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((BasketDto)null);

            var result = await this._controller.GetById(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetByUserId_Found_ReturnsOk()
        {
            var userId = Guid.NewGuid();
            var basket = new BasketDto { Id = 2 };
            this._basketServiceMock.Setup(s => s.GetByUserIdAsync(userId)).ReturnsAsync(basket);

            var result = await this._controller.GetByUserId(userId);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(basket, ok.Value);
        }

        [Test]
        public async Task GetByUserId_NotFound_ReturnsNotFound()
        {
            var userId = Guid.NewGuid();
            this._basketServiceMock.Setup(s => s.GetByUserIdAsync(userId)).ReturnsAsync((BasketDto)null);

            var result = await this._controller.GetByUserId(userId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Create_InvalidModelState_ReturnsBadRequest()
        {
            this._controller.ModelState.AddModelError("Test", "Invalid");
            var dto = new CreateBasketDto();

            var result = await this._controller.Create(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_Valid_ReturnsCreatedAtAction()
        {
            var dto = new CreateBasketDto();
            var created = new BasketDto { Id = 10 };
            this._basketServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await this._controller.Create(dto);

            var createdAt = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAt);
            Assert.AreEqual(nameof(this._controller.GetById), createdAt.ActionName);
            Assert.AreEqual(created.Id, ((BasketDto)createdAt.Value).Id);
        }

        [Test]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var dto = new UpdateBasketDto { Id = 2 };
            var result = await this._controller.Update(1, dto);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(string.Format(BasketMismatch), badRequest.Value);
        }

        [Test]
        public async Task Update_InvalidModelState_ReturnsBadRequest()
        {
            var dto = new UpdateBasketDto { Id = 1 };
            this._controller.ModelState.AddModelError("Test", "Invalid");

            var result = await this._controller.Update(1, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Update_KeyNotFoundException_ReturnsNotFound()
        {
            var dto = new UpdateBasketDto { Id = 1 };
            this._basketServiceMock.Setup(s => s.UpdateAsync(dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await this._controller.Update(1, dto);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Update_Valid_ReturnsOk()
        {
            var dto = new UpdateBasketDto { Id = 1 };
            var updated = new BasketDto { Id = 1 };
            this._basketServiceMock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(updated);

            var result = await this._controller.Update(1, dto);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(updated, ok.Value);
        }

        [Test]
        public async Task Delete_Valid_ReturnsNoContent()
        {
            this._basketServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await this._controller.Delete(1);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Delete_KeyNotFoundException_ReturnsNotFound()
        {
            this._basketServiceMock.Setup(s => s.DeleteAsync(1)).ThrowsAsync(new KeyNotFoundException());

            var result = await this._controller.Delete(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}
