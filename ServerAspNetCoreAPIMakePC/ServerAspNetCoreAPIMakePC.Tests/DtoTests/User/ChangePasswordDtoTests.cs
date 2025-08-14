namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.User
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.User;

    public class ChangePasswordDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new ChangePasswordDto
            {
                OldPassword = "OldPassword1!",
                NewPassword = "NewPassword1!"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void Short_NewPassword_Fails_Validation()
        {
            var dto = new ChangePasswordDto
            {
                OldPassword = "OldPassword1!",
                NewPassword = "a"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.Contains("password")));
        }
    }
}