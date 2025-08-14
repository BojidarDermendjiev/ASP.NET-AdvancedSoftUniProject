namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.User
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.User;

    public class UpdateUserDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new UpdateUserDto
            {
                FullName = "Valid Name",
                Email = "valid@email.com",
                Role = "User",
                Password = "Str0ngPassword!"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(results);
        }

        [Test]
        public void Invalid_Email_Fails_Validation()
        {
            var dto = new UpdateUserDto
            {
                FullName = "Valid Name",
                Email = "invalid",
                Role = "User",
                Password = "Str0ngPassword!"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.Contains("email")));
        }
    }
}