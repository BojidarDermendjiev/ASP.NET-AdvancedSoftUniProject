namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Brand
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Brand;
    public class CreateBrandDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new CreateBrandDto
            {
                Name = "BrandName",
                Description = "Brand description goes here.",
                LogoUrl = "https://brand.logo/image.png"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(results);
        }

        [Test]
        public void Name_Too_Short_Fails_Validation()
        {
            var dto = new CreateBrandDto
            {
                Name = string.Empty, 
                Description = "Valid brand description.",
                LogoUrl = "https://brand.logo/image.png"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("name")));
        }

        [Test]
        public void LogoUrl_Too_Long_Fails_Validation()
        {
            var dto = new CreateBrandDto
            {
                Name = "ValidBrand",
                Description = "Valid description",
                LogoUrl = new string('a', 1000) 
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("logo")));
        }
    }
}