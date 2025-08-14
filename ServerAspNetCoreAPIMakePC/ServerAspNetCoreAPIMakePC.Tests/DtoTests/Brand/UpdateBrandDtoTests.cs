namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Brand
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Brand;
    public class UpdateBrandDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new UpdateBrandDto
            {
                Id = 2,
                Name = "UpdatedBrand",
                Description = "Updated brand description.",
                LogoUrl = "https://brand.logo/updated.png"
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
            var dto = new UpdateBrandDto
            {
                Name = "BrandX",
                Description = "Some desc",
                LogoUrl = "https://brand.logo/x.png"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void Description_Too_Short_Fails_Validation()
        {
            var dto = new UpdateBrandDto
            {
                Id = 3,
                Name = "BrandY",
                Description = string.Empty, 
                LogoUrl = "https://brand.logo/y.png"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("description")));
        }
    }
}