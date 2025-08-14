namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.User
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.User;
    public class RegisterUserDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new RegisterUserDto
            {
                Email = "test@example.com",
                Password = "Str0ngPassword!",
                ConfirmPassword = "Str0ngPassword!",
                FullName = "Valid Name"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void Passwords_Do_Not_Match_Fails_Validation()
        {
            var dto = new RegisterUserDto
            {
                Email = "test@example.com",
                Password = "Str0ngPassword!",
                ConfirmPassword = "DifferentPassword!",
                FullName = "Valid Name"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.Contains("match")));
        }
    }
}