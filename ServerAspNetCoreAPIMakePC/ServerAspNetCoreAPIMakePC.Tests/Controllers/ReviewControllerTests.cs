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
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Review;

    [TestFixture]
    public class ReviewControllerTests
    {
        private Mock<IReviewService> _reviewServiceMock;
        private ReviewController _controller;

        private const string ReviewNotFound = "Review not found.";
        private const string ReviewInvalid = "Review is invalid.";
        private const string ReviewIdMismatch = "Review ID mismatch.";

        [SetUp]
        public void SetUp()
        {
            this._reviewServiceMock = new Mock<IReviewService>();
            this._controller = new ReviewController(this._reviewServiceMock.Object);
        }

        [Test]
        public async Task GetAll_ReturnsOkWithReviews()
        {
            var reviews = new List<ReviewDto> { new ReviewDto { Id = 1 } };
            this._reviewServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(reviews);

            var result = await _controller.GetAll();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreSame(reviews, okResult.Value);
        }

        [Test]
        public async Task GetAll_NoReviews_ReturnsNotFound()
        {
            this._reviewServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ReviewDto>());

            var result = await _controller.GetAll();

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.AreEqual(string.Format(ReviewNotFound), notFound.Value);
        }

        [Test]
        public async Task GetById_Found_ReturnsOk()
        {
            var review = new ReviewDto { Id = 1 };
            this._reviewServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(review);

            var result = await _controller.GetById(1);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreSame(review, okResult.Value);
        }

        [Test]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            this._reviewServiceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync((ReviewDto)null);

            var result = await _controller.GetById(1);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.AreEqual(string.Format(ReviewNotFound), notFound.Value);
        }

        [Test]
        public async Task GetByProductId_Found_ReturnsOk()
        {
            var productId = Guid.NewGuid();
            var reviews = new List<ReviewDto> { new ReviewDto { Id = 2 } };
            this._reviewServiceMock.Setup(s => s.GetByProductIdAsync(productId)).ReturnsAsync(reviews);

            var result = await _controller.GetByProductId(productId);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(reviews, ok.Value);
        }

        [Test]
        public async Task GetByProductId_None_ReturnsNotFound()
        {
            var productId = Guid.NewGuid();
            this._reviewServiceMock.Setup(s => s.GetByProductIdAsync(productId)).ReturnsAsync(new List<ReviewDto>());

            var result = await _controller.GetByProductId(productId);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.AreEqual(string.Format(ReviewNotFound), notFound.Value);
        }

        [Test]
        public async Task GetByUserId_Found_ReturnsOk()
        {
            var userId = Guid.NewGuid();
            var reviews = new List<ReviewDto> { new ReviewDto { Id = 3 } };
            this._reviewServiceMock.Setup(s => s.GetByUserIdAsync(userId)).ReturnsAsync(reviews);

            var result = await _controller.GetByUserId(userId);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(reviews, ok.Value);
        }

        [Test]
        public async Task GetByUserId_None_ReturnsNotFound()
        {
            var userId = Guid.NewGuid();
            this._reviewServiceMock.Setup(s => s.GetByUserIdAsync(userId)).ReturnsAsync(new List<ReviewDto>());

            var result = await _controller.GetByUserId(userId);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.AreEqual(string.Format(ReviewNotFound), notFound.Value);
        }

        [Test]
        public async Task Create_NullReview_ReturnsBadRequest()
        {
            var result = await _controller.Create(null);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(string.Format(ReviewInvalid), badRequest.Value);
        }

        [Test]
        public async Task Create_InvalidModelState_ReturnsBadRequest()
        {
            this._controller.ModelState.AddModelError("Test", "Invalid");
            var dto = new CreateReviewDto();

            var result = await this._controller.Create(dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_Valid_ReturnsCreatedAtAction()
        {
            var dto = new CreateReviewDto();
            var created = new ReviewDto { Id = 10 };
            this._reviewServiceMock.Setup(s => s.CreateAsync(dto)).ReturnsAsync(created);

            var result = await this._controller.Create(dto);

            var createdAt = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAt);
            Assert.AreEqual(nameof(this._controller.GetById), createdAt.ActionName);
            Assert.AreEqual(created.Id, ((ReviewDto)createdAt.Value).Id);
        }

        [Test]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var dto = new UpdateReviewDto { Id = 2 };
            var result = await this._controller.Update(1, dto);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual(string.Format(ReviewIdMismatch), badRequest.Value);
        }

        [Test]
        public async Task Update_InvalidModelState_ReturnsBadRequest()
        {
            var dto = new UpdateReviewDto { Id = 1 };
            this._controller.ModelState.AddModelError("Test", "Invalid");

            var result = await this._controller.Update(1, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            var dto = new UpdateReviewDto { Id = 1 };

            this._reviewServiceMock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync((ReviewDto)null);


            var notFound = (IActionResult?)await _controller.Update(1, dto) as NotFoundObjectResult;

            Assert.IsNotNull(notFound);
            Assert.AreEqual(string.Format(ReviewNotFound), notFound.Value);
        }

        [Test]
        public async Task Update_KeyNotFoundException_ReturnsNotFound()
        {
            var dto = new UpdateReviewDto { Id = 1 };
            this._reviewServiceMock.Setup(s => s.UpdateAsync(dto)).ThrowsAsync(new KeyNotFoundException("custom not found"));

            var result = await this._controller.Update(1, dto);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.AreEqual("custom not found", notFound.Value);
        }

        [Test]
        public async Task Update_Valid_ReturnsOk()
        {
            var dto = new UpdateReviewDto { Id = 1 };
            var updated = new ReviewDto { Id = 1 };
            this._reviewServiceMock.Setup(s => s.UpdateAsync(dto)).ReturnsAsync(updated);

            var result = await this._controller.Update(1, dto);

            var ok = result as OkObjectResult;
            Assert.IsNotNull(ok);
            Assert.AreSame(updated, ok.Value);
        }

        [Test]
        public async Task Delete_Valid_ReturnsNoContent()
        {
            this._reviewServiceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await this._controller.Delete(1);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Delete_KeyNotFoundException_ReturnsNotFound()
        {
            this._reviewServiceMock.Setup(s => s.DeleteAsync(1)).ThrowsAsync(new KeyNotFoundException("custom not found"));

            var result = await this._controller.Delete(1);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.AreEqual("custom not found", notFound.Value);
        }
    }
}
