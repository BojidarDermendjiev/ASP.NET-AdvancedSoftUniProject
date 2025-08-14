namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.User
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.User;

    public class LoginDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new LoginDto
            {
                Email = "test@example.com",
                Password = "TestPassword1"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void Missing_Email_Fails_Validation()
        {
            var dto = new LoginDto
            {
                Email = "",
                Password = "TestPassword1"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
        }
    }
}