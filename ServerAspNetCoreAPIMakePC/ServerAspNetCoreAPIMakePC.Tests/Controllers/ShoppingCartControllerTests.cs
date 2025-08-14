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
    using ServerAspNetCoreAPIMakePC.Application.Interfaces;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart;

    [TestFixture]
    public class ShoppingCartControllerTests
    {
        private Mock<IShoppingCartService> _shoppingCartServiceMock;
        private ShoppingCartController _controller;

        [SetUp]
        public void SetUp()
        {
            this._shoppingCartServiceMock = new Mock<IShoppingCartService>();
            this._controller = new ShoppingCartController(this._shoppingCartServiceMock.Object);
        }

        [Test]
        public async Task GetCartByUserId_Found_ReturnsOk()
        {
            var userId = Guid.NewGuid();
            var cart = new ShoppingCartDto { UserId = userId };
            this._shoppingCartServiceMock.Setup(s => s.GetCartByUserIdAsync(userId)).ReturnsAsync(cart);

            var result = await this._controller.GetCartByUserId(userId);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(cart, ok.Value);
        }

        [Test]
        public async Task GetCartByUserId_NotFound_ReturnsNotFound()
        {
            var userId = Guid.NewGuid();
            this._shoppingCartServiceMock.Setup(s => s.GetCartByUserIdAsync(userId)).ReturnsAsync((ShoppingCartDto)null);

            var result = await this._controller.GetCartByUserId(userId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task AddItem_Success_ReturnsOk()
        {
            var dto = new AddBasketItemDto();
            var cart = new ShoppingCartDto();
            this._shoppingCartServiceMock.Setup(s => s.AddItemAsync(dto)).ReturnsAsync(cart);

            var result = await this._controller.AddItem(dto);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(cart, ok.Value);
        }

        [Test]
        public async Task AddItem_ThrowsException_ReturnsBadRequest()
        {
            var dto = new AddBasketItemDto();
            this._shoppingCartServiceMock.Setup(s => s.AddItemAsync(dto)).ThrowsAsync(new Exception("error123"));

            var result = await this._controller.AddItem(dto);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual("error123", badRequest.Value);
        }

        [Test]
        public async Task RemoveItem_ReturnsOk()
        {
            var dto = new RemoveBasketItemDto();
            var cart = new ShoppingCartDto();
            this._shoppingCartServiceMock.Setup(s => s.RemoveItemAsync(dto)).ReturnsAsync(cart);

            var result = await this._controller.RemoveItem(dto);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(cart, ok.Value);
        }

        [Test]
        public async Task ClearCart_Success_ReturnsNoContent()
        {
            var userId = Guid.NewGuid();
            this._shoppingCartServiceMock.Setup(s => s.ClearCartAsync(userId)).Returns(Task.CompletedTask);

            var result = await  this._controller.ClearCart(userId);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task ClearCart_ThrowsException_ReturnsBadRequest()
        {
            var userId = Guid.NewGuid();
            this._shoppingCartServiceMock.Setup(s => s.ClearCartAsync(userId)).ThrowsAsync(new Exception("clear-error"));

            var result = await this._controller.ClearCart(userId);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual("clear-error", badRequest.Value);
        }
    }
}