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
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Brand;

    [TestFixture]
    public class BrandControllerTests
    {
        private Mock<IBrandService> _brandServiceMock;
        private BrandController _controller;

        private const string IdMismatchUserFriendly = "Id mismatch.";

        [SetUp]
        public void SetUp()
        {
            this._brandServiceMock = new Mock<IBrandService>();
            this._controller = new BrandController(this._brandServiceMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkWithBrands()
        {
            var brands = new List<BrandDto> { new BrandDto { Id = 1 } };
            this._brandServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(brands);

            var result = await this._controller.GetAll();

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(brands, ok.Value);
        }

        [Test]
        public async Task GetById_Found_ReturnsOk()
        {
            var brand = new BrandDto { Id = 1 };
            this._brandServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(brand);

            var result = await this._controller.GetById(1);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(brand, ok.Value);
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            this._brandServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((BrandDto)null);

            var result = await this._controller.GetById(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Create_InvalidModelState_ReturnsBadRequest()
        {
            this._controller.ModelState.AddModelError("Test", "Invalid");
            var dto = new CreateBrandDto();

            var result = await this._controller.Create(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_Valid_ReturnsCreatedAtAction()
        {
            var dto = new CreateBrandDto();
            var created = new BrandDto { Id = 10 };
            this._brandServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await this._controller.Create(dto);

            var createdAt = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAt);
            Assert.AreEqual(nameof(this._controller.GetById), createdAt.ActionName);
            Assert.AreEqual(created.Id, ((BrandDto)createdAt.Value).Id);
        }

        [Test]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var dto = new UpdateBrandDto { Id = 2 };
            var result = await this._controller.Update(1, dto);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(string.Format(IdMismatchUserFriendly), badRequest.Value);
        }

        [Test]
        public async Task Update_InvalidModelState_ReturnsBadRequest()
        {
            var dto = new UpdateBrandDto { Id = 1 };
            this._controller.ModelState.AddModelError("Test", "Invalid");

            var result = await this._controller.Update(1, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Update_KeyNotFoundException_ReturnsNotFound()
        {
            var dto = new UpdateBrandDto { Id = 1 };
            this._brandServiceMock.Setup(s => s.UpdateAsync(dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await this._controller.Update(1, dto);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Update_Valid_ReturnsOk()
        {
            var dto = new UpdateBrandDto { Id = 1 };
            var updated = new BrandDto { Id = 1 };
            this._brandServiceMock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(updated);

            var result = await this._controller.Update(1, dto);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(updated, ok.Value);
        }

        [Test]
        public async Task Delete_Valid_ReturnsNoContent()
        {
            this._brandServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await this._controller.Delete(1);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Delete_KeyNotFoundException_ReturnsNotFound()
        {
            this._brandServiceMock.Setup(s => s.DeleteAsync(1)).ThrowsAsync(new KeyNotFoundException());

            var result = await this._controller.Delete(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}