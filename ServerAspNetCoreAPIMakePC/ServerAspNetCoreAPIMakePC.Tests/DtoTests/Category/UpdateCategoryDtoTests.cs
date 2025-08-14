namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Category
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Category;

    public class UpdateCategoryDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new UpdateCategoryDto
            {
                Id = 2,
                Name = "GPUs",
                Description = "Graphics Processing Units"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(results);
        }

        [Test]
        public void Missing_Id_Fails_Validation()
        {
            var dto = new UpdateCategoryDto
            {
                Name = "GPUs",
                Description = "Graphics Processing Units"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void Name_Too_Short_Fails_Validation()
        {
            var dto = new UpdateCategoryDto
            {
                Id = 3,
                Name = string.Empty,
                Description = "Valid description"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("name")));
        }

        [Test]
        public void Description_Too_Short_Fails_Validation()
        {
            var dto = new UpdateCategoryDto
            {
                Id = 4,
                Name = "Motherboards",
                Description = string.Empty
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("description")));
        }
    }
}