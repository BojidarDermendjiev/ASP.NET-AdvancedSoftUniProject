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
    using Application.DTOs.Order;
    
    [TestFixture]
    public class OrderControllerTests
    {
        private Mock<IOrderService> _orderServiceMock;
        private OrderController _controller;

        private const string OrderMismatch = "Order ID mismatch.";

        [SetUp]
        public void SetUp()
        {
            this._orderServiceMock = new Mock<IOrderService>();
            this._controller = new OrderController(this._orderServiceMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkWithOrders()
        {
            var orders = new List<OrderDto> { new OrderDto { Id = 1 } };
            this._orderServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(orders);

            var result = await _controller.GetAll();

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(orders, ok.Value);
        }

        [Test]
        public async Task GetByUserId_ReturnsOkWithOrders()
        {
            var userId = Guid.NewGuid();
            var orders = new List<OrderDto> { new OrderDto { Id = 2 } };
            this._orderServiceMock.Setup(s => s.GetByUserIdAsync(userId)).ReturnsAsync(orders);

            var result = await this._controller.GetByUserId(userId);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(orders, ok.Value);
        }

        [Test]
        public async Task GetById_Found_ReturnsOk()
        {
            var order = new OrderDto { Id = 3 };
            this._orderServiceMock.Setup(s => s.GetByIdAsync(3)).ReturnsAsync(order);

            var result = await this._controller.GetById(3);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(order, ok.Value);
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            this._orderServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((OrderDto)null);

            var result = await this._controller.GetById(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Create_InvalidModelState_ReturnsBadRequest()
        {
            this._controller.ModelState.AddModelError("Test", "Invalid");
            var dto = new CreateOrderDto();

            var result = await this._controller.Create(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_Valid_ReturnsCreatedAtAction()
        {
            var dto = new CreateOrderDto();
            var created = new OrderDto { Id = 10 };
            this._orderServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await  this._controller.Create(dto);

            var createdAt = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAt);
            Assert.AreEqual(nameof(this._controller.GetById), createdAt.ActionName);
            Assert.AreEqual(created.Id, ((OrderDto)createdAt.Value).Id);
        }

        [Test]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var dto = new UpdateOrderDto { Id = 2 };
            var result = await this._controller.Update(1, dto);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(string.Format(OrderMismatch), badRequest.Value);
        }

        [Test]
        public async Task Update_InvalidModelState_ReturnsBadRequest()
        {
            var dto = new UpdateOrderDto { Id = 1 };
            this._controller.ModelState.AddModelError("Test", "Invalid");

            var result = await this._controller.Update(1, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Update_KeyNotFoundException_ReturnsNotFound()
        {
            var dto = new UpdateOrderDto { Id = 1 };
            this._orderServiceMock.Setup(s => s.UpdateAsync(dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await this._controller.Update(1, dto);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Update_Valid_ReturnsOk()
        {
            var dto = new UpdateOrderDto { Id = 1 };
            var updated = new OrderDto { Id = 1 };
            this._orderServiceMock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(updated);

            var result = await this._controller.Update(1, dto);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(updated, ok.Value);
        }

        [Test]
        public async Task Delete_CallsServiceAndReturnsNoContent()
        {
            this._orderServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await this._controller.Delete(1);

            Assert.IsInstanceOf<NoContentResult>(result);
            this._orderServiceMock.Verify(s => s.DeleteAsync(1), Times.Once);
        }
    }
}