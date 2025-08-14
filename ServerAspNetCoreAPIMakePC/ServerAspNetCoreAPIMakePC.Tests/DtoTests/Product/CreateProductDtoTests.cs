namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Product
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Product;

    public class CreateProductDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new CreateProductDto
            {
                Name = "Product1",
                Description = "A valid product description goes here.",
                Price = 25.95m,
                BrandId = 1,
                CategoryId = 2,
                StockQuantity = 30
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(results);
        }

        [Test]
        public void Invalid_Name_Fails_Validation()
        {
            var dto = new CreateProductDto
            {
                Name = string.Empty, 
                Description = "A valid product description goes here.",
                Price = 25.95m,
                BrandId = 1,
                CategoryId = 2
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.Contains("name")));
        }

        [Test]
        public void Invalid_Price_Fails_Validation()
        {
            var dto = new CreateProductDto
            {
                Name = "Product1",
                Description = "A valid product description goes here.",
                Price = -100m, 
                BrandId = 1,
                CategoryId = 2
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.Contains("price")));
        }
    }
}