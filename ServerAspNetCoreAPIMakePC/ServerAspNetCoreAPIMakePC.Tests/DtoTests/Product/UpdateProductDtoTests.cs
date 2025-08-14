namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Product
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Product;
    public class UpdateProductDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new UpdateProductDto
            {
                Name = "Updated Product",
                Type = "Electronics",
                Brand = "BrandX",
                Price = 12.50m,
                Stock = 100,
                Description = "Updated product description.",
                Specs = "Specs details",
                ImageUrl = "https://example.com/image.png",
                CategoryId = 2
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void Invalid_ImageUrl_Fails_Validation()
        {
            var dto = new UpdateProductDto
            {
                Name = "Product",
                ImageUrl = "not-a-valid-url"
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.Contains("image")));
        }

        [Test]
        public void Too_Short_Description_Fails_Validation()
        {
            var dto = new UpdateProductDto
            {
                Description = string.Empty
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.Contains("description")));
        }
    }
}