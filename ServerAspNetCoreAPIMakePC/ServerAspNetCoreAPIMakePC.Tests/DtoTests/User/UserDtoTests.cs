namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.User
{
    using System;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.User;
    public class UserDtoTests
    {
        [Test]
        public void Can_Construct_And_Read_Properties()
        {
            var id = Guid.NewGuid();
            var dto = new UserDto
            {
                Id = id,
                Email = "test@example.com",
                FullName = "Test User",
                Role = "User"
            };

            Assert.AreEqual(id, dto.Id);
            Assert.AreEqual("test@example.com", dto.Email);
            Assert.AreEqual("Test User", dto.FullName);
            Assert.AreEqual("User", dto.Role);
        }
    }
}