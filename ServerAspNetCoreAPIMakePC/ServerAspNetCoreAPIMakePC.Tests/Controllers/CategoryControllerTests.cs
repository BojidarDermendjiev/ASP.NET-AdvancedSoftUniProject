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
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Category;

    [TestFixture]
    public class CategoryControllerTests
    {
        private Mock<ICategoryService> _categoryServiceMock;
        private CategoryController _controller;

        // Adjust this error message to match your static error message if needed
        private const string CategoryMismatch = "Category ID mismatch.";

        [SetUp]
        public void SetUp()
        {
            this._categoryServiceMock = new Mock<ICategoryService>();
            this._controller = new CategoryController(this._categoryServiceMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkWithCategories()
        {
            var categories = new List<CategoryDto> { new CategoryDto { Id = 1 } };
            this._categoryServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(categories);

            var result = await this._controller.GetAll();

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(categories, ok.Value);
        }

        [Test]
        public async Task GetById_Found_ReturnsOk()
        {
            var category = new CategoryDto { Id = 1 };
            this._categoryServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(category);

            var result = await this._controller.GetById(1);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(category, ok.Value);
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            this._categoryServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((CategoryDto)null);

            var result = await this._controller.GetById(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Create_InvalidModelState_ReturnsBadRequest()
        {
            this._controller.ModelState.AddModelError("Test", "Invalid");
            var dto = new CreateCategoryDto();

            var result = await this._controller.Create(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_Valid_ReturnsCreatedAtAction()
        {
            var dto = new CreateCategoryDto();
            var created = new CategoryDto { Id = 10 };
            this._categoryServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await this._controller.Create(dto);

            var createdAt = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAt);
            Assert.AreEqual(nameof(this._controller.GetById), createdAt.ActionName);
            Assert.AreEqual(created.Id, ((CategoryDto)createdAt.Value).Id);
        }

        [Test]
        public async Task Update_InvalidModelState_ReturnsBadRequest()
        {
            var dto = new UpdateCategoryDto { Id = 1 };
            this._controller.ModelState.AddModelError("Test", "Invalid");

            var result = await this._controller.Update(1, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var dto = new UpdateCategoryDto { Id = 2 };

            var result = await this._controller.Update(1, dto);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(string.Format(CategoryMismatch), badRequest.Value);
        }

        [Test]
        public async Task Update_KeyNotFoundException_ReturnsNotFound()
        {
            var dto = new UpdateCategoryDto { Id = 1 };
            this._categoryServiceMock.Setup(s => s.UpdateAsync(dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await this._controller.Update(1, dto);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Update_Valid_ReturnsOk()
        {
            var dto = new UpdateCategoryDto { Id = 1 };
            var updated = new CategoryDto { Id = 1 };
            this._categoryServiceMock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(updated);

            var result = await this._controller.Update(1, dto);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(updated, ok.Value);
        }

        [Test]
        public async Task Delete_Valid_ReturnsNoContent()
        {
            this._categoryServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await this._controller.Delete(1);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Delete_KeyNotFoundException_ReturnsNotFound()
        {
            this._categoryServiceMock.Setup(s => s.DeleteAsync(1)).ThrowsAsync(new KeyNotFoundException());

            var result = await this._controller.Delete(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}