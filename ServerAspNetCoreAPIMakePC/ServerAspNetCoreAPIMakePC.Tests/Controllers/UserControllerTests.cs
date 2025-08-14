namespace ServerAspNetCoreAPIMakePC.Tests.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;

    using Moq;
    using NUnit.Framework;

    using API.Controllers;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.User;
    using ServerAspNetCoreAPIMakePC.Application.Interfaces;

    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private UserController _controller;

        [SetUp]
        public void SetUp()
        {
            this._userServiceMock = new Mock<IUserService>();
            this._controller = new UserController(this._userServiceMock.Object);
        }

        [Test]
        public async Task Register_ReturnsOk_WhenRegistrationSucceeds()
        {
            var registerDto = new RegisterUserDto { Email = "a@a.com", Password = "pass", ConfirmPassword = "pass", FullName = "Test" };
            var userDto = new UserDto { Id = Guid.NewGuid(), Email = "a@a.com", FullName = "Test", Role = "User" };
            var token = "token";
            this._userServiceMock.Setup(x => x.RegisterUserAsync(registerDto)).ReturnsAsync(userDto);
            this._userServiceMock.Setup(x => x.GenerateJwtToken(userDto)).Returns(token);

            var result = await this._controller.Register(registerDto) as OkObjectResult;

            Assert.IsNotNull(result);
            dynamic model = result.Value;
            Assert.AreEqual(userDto, model.user);
            Assert.AreEqual(token, model.token);
        }

        [Test]
        public async Task Register_ReturnsConflict_OnInvalidOperationException()
        {
            var registerDto = new RegisterUserDto();
            this._userServiceMock.Setup(x => x.RegisterUserAsync(registerDto)).ThrowsAsync(new InvalidOperationException("exists"));

            var result = await this._controller.Register(registerDto) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(409, result.StatusCode);
            Assert.That(result.Value.ToString(), Does.Contain("exists"));
        }

        [Test]
        public async Task Register_ReturnsBadRequest_OnArgumentException()
        {
            var registerDto = new RegisterUserDto();
            this._userServiceMock.Setup(x => x.RegisterUserAsync(registerDto)).ThrowsAsync(new ArgumentException("bad"));

            var result = await this._controller.Register(registerDto) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.That(result.Value.ToString(), Does.Contain("bad"));
        }

        [Test]
        public async Task Authenticate_ReturnsUnauthorized_WhenUserNull()
        {
            var dto = new AuthenticateUserDto { Email = "z@z.com", Password = "wrong" };
            this._userServiceMock.Setup(x => x.AuthenticateUserAsync(It.IsAny<Domain.ValueObjects.Email>(), dto.Password)).ReturnsAsync((UserDto)null);

            var result = await this._controller.Authenticate(dto) as ObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);
            Assert.That(result.Value.ToString(), Does.Contain("Invalid credentials"));
        }

        [Test]
        public async Task Authenticate_ReturnsOk_WithTokenAndUser()
        {
            var dto = new AuthenticateUserDto { Email = "a@a.com", Password = "pass" };
            var userDto = new UserDto { Id = Guid.NewGuid(), Email = "a@a.com", FullName = "Test", Role = "User" };
            var token = "token";
            this._userServiceMock.Setup(x => x.AuthenticateUserAsync(It.IsAny<Domain.ValueObjects.Email>(), dto.Password)).ReturnsAsync(userDto);
            this._userServiceMock.Setup(x => x.GenerateJwtToken(userDto)).Returns(token);

            var result = await this._controller.Authenticate(dto) as OkObjectResult;

            Assert.IsNotNull(result);
            dynamic model = result.Value;
            Assert.AreEqual(token, model.token);
            Assert.AreEqual(userDto, model.user);
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenUserNull()
        {
            var id = Guid.NewGuid();
            this._userServiceMock.Setup(x => x.GetUserByIdAsync(id)).ReturnsAsync((UserDto)null);

            var result = await this._controller.GetById(id);

            Assert.IsInstanceOf<NotFoundObjectResult>(result.Result);
        }

        [Test]
        public async Task GetById_ReturnsOk_WhenUserExists()
        {
            var id = Guid.NewGuid();
            var userDto = new UserDto { Id = id, Email = "a@a.com", FullName = "Test", Role = "User" };
            this._userServiceMock.Setup(x => x.GetUserByIdAsync(id)).ReturnsAsync(userDto);

            var result = await this._controller.GetById(id);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.AreEqual(userDto, okResult.Value);
        }

        [Test]
        public async Task Update_ReturnsNoContent_OnSuccess()
        {
            var id = Guid.NewGuid();
            var dto = new UpdateUserDto { Email = "a@a.com", FullName = "Test", Role = "User" };
            this._userServiceMock.Setup(x => x.UpdateUserAsync(id, dto)).Returns(Task.CompletedTask);

            var result = await this._controller.Update(id, dto);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Update_ReturnsNotFound_OnInvalidOperationException()
        {
            var id = Guid.NewGuid();
            var dto = new UpdateUserDto();
            this._userServiceMock.Setup(x => x.UpdateUserAsync(id, dto)).ThrowsAsync(new InvalidOperationException("not found"));

            var result = await this._controller.Update(id, dto);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsNoContent_OnSuccess()
        {
            var id = Guid.NewGuid();
            this._userServiceMock.Setup(x => x.DeleteUserAsync(id)).Returns(Task.CompletedTask);

            var result = await this._controller.Delete(id);

            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_OnException()
        {
            var id = Guid.NewGuid();
            this._userServiceMock.Setup(x => x.DeleteUserAsync(id)).ThrowsAsync(new Exception("not found"));

            var result = await this._controller.Delete(id);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task ChangePassword_ReturnsOk_OnSuccess()
        {
            var id = Guid.NewGuid();
            var dto = new ChangePasswordDto { OldPassword = "old", NewPassword = "new" };
            this._userServiceMock.Setup(x => x.ChangePasswordAsync(id, dto.OldPassword, dto.NewPassword)).Returns(Task.CompletedTask);

            var result = await this._controller.ChangePassword(id, dto);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task ChangePassword_ReturnsNotFound_OnKeyNotFoundException()
        {
            var id = Guid.NewGuid();
            var dto = new ChangePasswordDto();
            this._userServiceMock.Setup(x => x.ChangePasswordAsync(id, dto.OldPassword, dto.NewPassword)).ThrowsAsync(new KeyNotFoundException("not found"));

            var result = await this._controller.ChangePassword(id, dto);

            Assert.IsInstanceOf<NotFoundObjectResult>(result);
        }

        [Test]
        public async Task ChangePassword_ReturnsBadRequest_OnInvalidOperationException()
        {
            var id = Guid.NewGuid();
            var dto = new ChangePasswordDto();
            this._userServiceMock.Setup(x => x.ChangePasswordAsync(id, dto.OldPassword, dto.NewPassword)).ThrowsAsync(new InvalidOperationException("bad"));

            var result = await this._controller.ChangePassword(id, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task ChangePassword_ReturnsBadRequest_OnArgumentException()
        {
            var id = Guid.NewGuid();
            var dto = new ChangePasswordDto();
            this._userServiceMock.Setup(x => x.ChangePasswordAsync(id, dto.OldPassword, dto.NewPassword)).ThrowsAsync(new ArgumentException("bad"));

            var result = await this._controller.ChangePassword(id, dto);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UploadAvatar_ReturnsBadRequest_IfNoForm()
        {
            var id = Guid.NewGuid();
            var context = new DefaultHttpContext();
            context.Request.ContentType = "application/json";
            this._controller.ControllerContext = new ControllerContext { HttpContext = context };

            var result = await this._controller.UploadAvatar(id);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UploadAvatar_ReturnsBadRequest_IfNoFiles()
        {
            var id = Guid.NewGuid();
            var form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(), new FormFileCollection());
            var context = new DefaultHttpContext();
            context.Request.ContentType = "multipart/form-data";
            context.Request.Form = form;
            this._controller.ControllerContext = new ControllerContext { HttpContext = context };

            var result = await this._controller.UploadAvatar(id);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UploadAvatar_ReturnsBadRequest_IfFileEmpty()
        {
            var id = Guid.NewGuid();
            var mem = new MemoryStream();
            var file = new FormFile(mem, 0, 0, "file", "file.png");
            var files = new FormFileCollection { file };
            var form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(), files);
            var context = new DefaultHttpContext();
            context.Request.ContentType = "multipart/form-data";
            context.Request.Form = form;
            this._controller.ControllerContext = new ControllerContext { HttpContext = context };

            var result = await this._controller.UploadAvatar(id);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task UploadAvatar_ReturnsOk_WhenSuccess()
        {
            var id = Guid.NewGuid();
            var buffer = new byte[] { 1, 2, 3 };
            var mem = new MemoryStream(buffer);
            var file = new FormFile(mem, 0, buffer.Length, "file", "file.png");
            var files = new FormFileCollection { file };
            var form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(), files);
            var context = new DefaultHttpContext();
            context.Request.ContentType = "multipart/form-data";
            context.Request.Form = form;
            this._controller.ControllerContext = new ControllerContext { HttpContext = context };
            this._userServiceMock.Setup(x => x.UpdateAvatarAsync(id, It.IsAny<byte[]>(), "file.png")).Returns(Task.CompletedTask);

            var result = await this._controller.UploadAvatar(id);

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task GetClients_ReturnsOk_WithClients()
        {
            var clients = new List<UserDto>
            {
                new UserDto { Id = Guid.NewGuid(), Email = "a@a.com", FullName = "a", Role = "User" },
                new UserDto { Id = Guid.NewGuid(), Email = "b@b.com", FullName = "b", Role = "User" }
            };
            this._userServiceMock.Setup(x => x.GetClientsAsync()).ReturnsAsync(clients);

            var result = await _controller.GetClients() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(clients, result.Value);
        }
    }
}