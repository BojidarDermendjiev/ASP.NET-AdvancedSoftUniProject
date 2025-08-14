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
    using Application.DTOs.User;
    using ServerAspNetCoreAPIMakePC.Application.Settings;
    using ServerAspNetCoreAPIMakePC.Application.Services;

    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private IOptions<JwtSettings> _jwtSettings;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            this._userRepositoryMock = new Mock<IUserRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._jwtSettings = Options.Create(new JwtSettings
            {
                Secret = "test_secret_12345678901234567890", 
                Issuer = "issuer",
                Audience = "audience",
                LifespanMinutes = 60
            });

            this._userService = new UserService(this._userRepositoryMock.Object, this._mapperMock.Object, this._jwtSettings);
        }

        [Test]
        public async Task GetClientsAsync_Returns_Only_Users_With_UserRole_User()
        {
            var users = new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = new Email("client1@example.com"),
                    FullName = new FullName("Client One"),
                    Role = UserRole.User
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = new Email("admin@example.com"),
                    FullName = new FullName("Admin User"),
                    Role = UserRole.Admin
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = new Email("client2@example.com"),
                    FullName = new FullName("Client Two"),
                    Role = UserRole.User
                }
            };

            this._userRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(users);


            var result = await _userService.GetClientsAsync();

            Assert.IsNotNull(result);
            var clientList = result.ToList();
            Assert.AreEqual(2, clientList.Count);
            Assert.IsTrue(clientList.All(u => u.Role == UserRole.User.ToString()));
            Assert.IsTrue(clientList.Any(u => u.Email == "client1@example.com"));
            Assert.IsTrue(clientList.Any(u => u.Email == "client2@example.com"));
            Assert.IsFalse(clientList.Any(u => u.Email == "admin@example.com"));
        }
    }
}