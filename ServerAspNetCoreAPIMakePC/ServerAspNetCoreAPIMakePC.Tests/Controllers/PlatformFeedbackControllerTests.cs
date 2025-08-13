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
    using Application.DTOs.Feedback;

    [TestFixture]
    public class PlatformFeedbackControllerTests
    {
        private Mock<IPlatformFeedbackService> _feedbackServiceMock;
        private PlatformFeedbackController _controller;

        [SetUp]
        public void SetUp()
        {
            this._feedbackServiceMock = new Mock<IPlatformFeedbackService>();
            this._controller = new PlatformFeedbackController(this._feedbackServiceMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkWithFeedbacks()
        {
            var feedbacks = new List<PlatformFeedbackDto> { new PlatformFeedbackDto { Id = 1 } };
            this._feedbackServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(feedbacks);

            var result = await this._controller.GetAll();

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
            Assert.AreSame(feedbacks, ok.Value);
        }

        [Test]
        public async Task GetById_Found_ReturnsOk()
        {
            var feedback = new PlatformFeedbackDto { Id = 1 };
            this._feedbackServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(feedback);

            var result = await _controller.GetById(1);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreEqual(200, ok.StatusCode);
            Assert.AreSame(feedback, ok.Value);
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            this._feedbackServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((PlatformFeedbackDto)null);

            var result = await _controller.GetById(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetByUserId_Found_ReturnsOk()
        {
            var userId = Guid.NewGuid();
            var feedbacks = new List<PlatformFeedbackDto> { new PlatformFeedbackDto { Id = 2 } };
            this._feedbackServiceMock.Setup(s => s.GetByUserIdAsync(userId)).ReturnsAsync(feedbacks);

            var result = await _controller.GetByUserId(userId);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(feedbacks, ok.Value);
        }

        [Test]
        public async Task GetByUserId_None_ReturnsNotFound()
        {
            var userId = Guid.NewGuid();
            this._feedbackServiceMock.Setup(s => s.GetByUserIdAsync(userId)).ReturnsAsync(new List<PlatformFeedbackDto>());

            var result = await _controller.GetByUserId(userId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Create_InvalidModel_ReturnsBadRequest()
        {
            this._controller.ModelState.AddModelError("Test", "Invalid");

            var dto = new CreatePlatformFeedbackDto();
            var result = await this._controller.Create(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_Valid_ReturnsCreatedAtAction()
        {
            var dto = new CreatePlatformFeedbackDto();
            var created = new PlatformFeedbackDto { Id = 3 };
            this._feedbackServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await this._controller.Create(dto);

            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(nameof(this._controller.GetById), createdResult.ActionName);
            Assert.AreEqual(created.Id, ((PlatformFeedbackDto)createdResult.Value).Id);
        }

        [Test]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var dto = new UpdatePlatformFeedbackDto { Id = 2 };
            var result = await this._controller.Update(1, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Update_InvalidModelState_ReturnsBadRequest()
        {
            var dto = new UpdatePlatformFeedbackDto { Id = 1 };
            this._controller.ModelState.AddModelError("Test", "Invalid");

            var result = await this._controller.Update(1, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var dto = new UpdatePlatformFeedbackDto { Id = 1 };
            this._feedbackServiceMock.Setup(s => s.UpdateAsync(dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await this._controller.Update(1, dto);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task Update_Valid_ReturnsOk()
        {
            var dto = new UpdatePlatformFeedbackDto { Id = 1 };
            var updated = new PlatformFeedbackDto { Id = 1 };
            this._feedbackServiceMock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(updated);

            var result = await this._controller.Update(1, dto);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(updated, ok.Value);
        }

        [Test]
        public async Task Delete_Valid_ReturnsNoContent()
        {
            this._feedbackServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await this._controller.Delete(1);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Delete_NotFound_ReturnsNotFound()
        {
            this._feedbackServiceMock.Setup(s => s.DeleteAsync(1)).ThrowsAsync(new KeyNotFoundException());

            var result = await this._controller.Delete(1);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

    }
}