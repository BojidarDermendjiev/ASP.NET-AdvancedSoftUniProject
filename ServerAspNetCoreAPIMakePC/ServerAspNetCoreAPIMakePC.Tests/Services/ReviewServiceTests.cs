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
    using Application.Services;
    using Application.Settings;
    using Application.Interfaces;
    using Application.DTOs.Review;

    [TestFixture]
    public class ReviewServiceTests
    {
        private Mock<IReviewRepository> _reviewRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private ReviewService _reviewService;

        [SetUp]
        public void SetUp()
        {
            this._reviewRepositoryMock = new Mock<IReviewRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._reviewService = new ReviewService(this._reviewRepositoryMock.Object, this._mapperMock.Object);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsReviewDto_IfExists()
        {
            var review = new Review { Id = 1 };
            var reviewDto = new ReviewDto { Id = 1 };

            this._reviewRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(review);
            this._mapperMock.Setup(m => m.Map<ReviewDto>(review)).Returns(reviewDto);

            var result = await this._reviewService.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_IfNotExists()
        {
            this._reviewRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Review)null);

            var result = await this._reviewService.GetByIdAsync(1);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsMappedDtos()
        {
            var reviews = new List<Review> { new Review { Id = 1 }, new Review { Id = 2 } };
            var dtos = new List<ReviewDto> { new ReviewDto { Id = 1 }, new ReviewDto { Id = 2 } };

            this._reviewRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(reviews);
            this._mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(reviews)).Returns(dtos);

            var result = (await this._reviewService.GetAllAsync()).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
        }

        [Test]
        public async Task GetByProductIdAsync_ReturnsMappedDtos()
        {
            var productId = Guid.NewGuid();
            var reviews = new List<Review> { new Review { Id = 1, ProductId = productId } };
            var dtos = new List<ReviewDto> { new ReviewDto { Id = 1 } };

            this._reviewRepositoryMock.Setup(r => r.GetByProductIdAsync(productId)).ReturnsAsync(reviews);
            this._mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(reviews)).Returns(dtos);

            var result = (await this._reviewService.GetByProductIdAsync(productId)).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].Id);
        }

        [Test]
        public async Task GetByUserIdAsync_ReturnsMappedDtos()
        {
            var userId = Guid.NewGuid();
            var reviews = new List<Review> { new Review { Id = 1, UserId = userId } };
            var dtos = new List<ReviewDto> { new ReviewDto { Id = 1 } };

            this._reviewRepositoryMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(reviews);
            this._mapperMock.Setup(m => m.Map<IEnumerable<ReviewDto>>(reviews)).Returns(dtos);

            var result = (await this._reviewService.GetByUserIdAsync(userId)).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].Id);
        }

        [Test]
        public async Task CreateAsync_MapsAndAddsReview_ReturnsDto()
        {
            var createDto = new CreateReviewDto();
            var review = new Review();
            var reviewDto = new ReviewDto();

            this._mapperMock.Setup(m => m.Map<Review>(createDto)).Returns(review);
            this._reviewRepositoryMock.Setup(r => r.AddAsync(review)).Returns(Task.CompletedTask);
            this._mapperMock.Setup(m => m.Map<ReviewDto>(review)).Returns(reviewDto);

            var result = await this._reviewService.CreateAsync(createDto);

            Assert.IsNotNull(result);
            this._reviewRepositoryMock.Verify(r => r.AddAsync(review), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_UpdatesAndReturnsDto_IfExists()
        {
            var updateDto = new UpdateReviewDto { Id = 3 };
            var review = new Review { Id = 3 };
            var reviewDto = new ReviewDto { Id = 3 };

            this._reviewRepositoryMock.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync(review);
            this._mapperMock.Setup(m => m.Map(updateDto, review));
            this._reviewRepositoryMock.Setup(r => r.UpdateAsync(review)).Returns(Task.CompletedTask);
            this._mapperMock.Setup(m => m.Map<ReviewDto>(review)).Returns(reviewDto);

            var result = await this._reviewService.UpdateAsync(updateDto);

            Assert.AreEqual(3, result.Id);
            this._reviewRepositoryMock.Verify(r => r.UpdateAsync(review), Times.Once);
        }

        [Test]
        public void UpdateAsync_Throws_IfReviewNotFound()
        {
            var updateDto = new UpdateReviewDto { Id = 2 };
            this._reviewRepositoryMock.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync((Review)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _reviewService.UpdateAsync(updateDto));
        }

        [Test]
        public async Task DeleteAsync_CallsRepositoryDelete()
        {
            const int id = 5;
            this._reviewRepositoryMock.Setup(r => r.DeleteAsync(id)).Returns(Task.CompletedTask);

            await this._reviewService.DeleteAsync(id);

            this._reviewRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }
}