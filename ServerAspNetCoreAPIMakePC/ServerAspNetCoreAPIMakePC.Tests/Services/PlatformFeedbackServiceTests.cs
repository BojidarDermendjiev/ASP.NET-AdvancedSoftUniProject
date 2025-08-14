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
    using Application.DTOs.Feedback;

    [TestFixture]
    public class PlatformFeedbackServiceTests
    {
        private Mock<IPlatformFeedbackRepository> _feedbackRepositoryMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IMapper> _mapperMock;
        private PlatformFeedbackService _service;

        [SetUp]
        public void SetUp()
        {
            this._feedbackRepositoryMock = new Mock<IPlatformFeedbackRepository>();
            this._userServiceMock = new Mock<IUserService>();
            this._mapperMock = new Mock<IMapper>();
            this._service = new PlatformFeedbackService(
                this._feedbackRepositoryMock.Object,
                this._mapperMock.Object,
                this._userServiceMock.Object
            );
        }

        [Test]
        public async Task GetByIdAsync_ReturnsDtoWithUserName_IfFound()
        {
            var feedback = new PlatformFeedback { Id = 1, UserId = Guid.NewGuid() };
            var feedbackDto = new PlatformFeedbackDto { Id = 1 };
            var userDto = new Application.DTOs.User.UserDto { FullName = "Test User", Email = "test@email.com" };

            this._feedbackRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(feedback);
            this._mapperMock.Setup(m => m.Map<PlatformFeedbackDto>(feedback)).Returns(feedbackDto);
            this._userServiceMock.Setup(us => us.GetUserByIdAsync(feedback.UserId)).ReturnsAsync(userDto);

            var result = await this._service.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test User", result.UserName);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_IfNotFound()
        {
            this._feedbackRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((PlatformFeedback)null);

            var result = await this._service.GetByIdAsync(1);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllWithUserNames()
        {
            var userId = Guid.NewGuid();
            var feedbacks = new List<PlatformFeedback>
            {
                new PlatformFeedback { Id = 1, UserId = userId },
                new PlatformFeedback { Id = 2, UserId = userId }
            };
            var dtos = new List<PlatformFeedbackDto>
            {
                new PlatformFeedbackDto { Id = 1 },
                new PlatformFeedbackDto { Id = 2 }
            };
            var userDto = new Application.DTOs.User.UserDto { FullName = "User Name", Email = "u@email.com" };

            this._feedbackRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(feedbacks);
            this._mapperMock.Setup(m => m.Map<PlatformFeedbackDto>(It.IsAny<PlatformFeedback>()))
                .Returns<PlatformFeedback>(f => new PlatformFeedbackDto { Id = f.Id });
            this._userServiceMock.Setup(us => us.GetUserByIdAsync(userId)).ReturnsAsync(userDto);

            var result = (await this._service.GetAllAsync()).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(r => r.UserName == "User Name"));
        }

        [Test]
        public async Task GetByUserIdAsync_ReturnsFeedbacksWithUserName()
        {
            var userId = Guid.NewGuid();
            var feedbacks = new List<PlatformFeedback>
            {
                new PlatformFeedback { Id = 1, UserId = userId }
            };
            var userDto = new Application.DTOs.User.UserDto { FullName = "Name", Email = "e@mail.com" };

            this._feedbackRepositoryMock.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(feedbacks);
            this._mapperMock.Setup(m => m.Map<PlatformFeedbackDto>(It.IsAny<PlatformFeedback>()))
                .Returns<PlatformFeedback>(f => new PlatformFeedbackDto { Id = f.Id });
            this._userServiceMock.Setup(us => us.GetUserByIdAsync(userId)).ReturnsAsync(userDto);

            var result = (await this._service.GetByUserIdAsync(userId)).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Name", result[0].UserName);
        }

        [Test]
        public async Task CreateAsync_CreatesAndReturnsDtoWithUserName()
        {
            var createDto = new CreatePlatformFeedbackDto();
            var feedback = new PlatformFeedback { UserId = Guid.NewGuid() };
            var feedbackDto = new PlatformFeedbackDto();
            var userDto = new Application.DTOs.User.UserDto { FullName = "Test Name", Email = "test@e.com" };

            this._mapperMock.Setup(m => m.Map<PlatformFeedback>(createDto)).Returns(feedback);
            this._feedbackRepositoryMock.Setup(r => r.AddAsync(feedback)).Returns(Task.CompletedTask);
            this._mapperMock.Setup(m => m.Map<PlatformFeedbackDto>(feedback)).Returns(feedbackDto);
            this._userServiceMock.Setup(us => us.GetUserByIdAsync(feedback.UserId)).ReturnsAsync(userDto);

            var result = await this._service.CreateAsync(createDto);

            Assert.IsNotNull(result);
            Assert.AreEqual("Test Name", result.UserName);
        }

        [Test]
        public async Task UpdateAsync_Throws_IfNotFound()
        {
            var updateDto = new UpdatePlatformFeedbackDto { Id = 5 };
            this._feedbackRepositoryMock.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync((PlatformFeedback)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._service.UpdateAsync(updateDto));
        }

        [Test]
        public async Task UpdateAsync_UpdatesAndReturnsDtoWithUserName()
        {
            var updateDto = new UpdatePlatformFeedbackDto { Id = 2 };
            var feedback = new PlatformFeedback { Id = 2, UserId = Guid.NewGuid() };
            var feedbackDto = new PlatformFeedbackDto { Id = 2 };
            var userDto = new Application.DTOs.User.UserDto { FullName = "Upd Name", Email = "upd@e.com" };

            this._feedbackRepositoryMock.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync(feedback);
            this._mapperMock.Setup(m => m.Map(updateDto, feedback));
            this._feedbackRepositoryMock.Setup(r => r.UpdateAsync(feedback)).Returns(Task.CompletedTask);
            this._mapperMock.Setup(m => m.Map<PlatformFeedbackDto>(feedback)).Returns(feedbackDto);
            this._userServiceMock.Setup(us => us.GetUserByIdAsync(feedback.UserId)).ReturnsAsync(userDto);

            var result = await this._service.UpdateAsync(updateDto);

            Assert.IsNotNull(result);
            Assert.AreEqual("Upd Name", result.UserName);
            this._feedbackRepositoryMock.Verify(r => r.UpdateAsync(feedback), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Throws_IfNotFound()
        {
            this._feedbackRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((PlatformFeedback)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._service.DeleteAsync(11));
        }

        [Test]
        public async Task DeleteAsync_Deletes_IfFound()
        {
            var feedback = new PlatformFeedback { Id = 3 };
            this._feedbackRepositoryMock.Setup(r => r.GetByIdAsync(feedback.Id)).ReturnsAsync(feedback);
            this._feedbackRepositoryMock.Setup(r => r.DeleteAsync(feedback.Id)).Returns(Task.CompletedTask);

            await this._service.DeleteAsync(feedback.Id);

            this._feedbackRepositoryMock.Verify(r => r.DeleteAsync(feedback.Id), Times.Once);
        }
    }
}