namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.ShoppingCart
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart;
    public class AddBasketItemDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new AddBasketItemDto
            {
                UserId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 2
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(results);
        }

        [Test]
        public void Missing_UserId_Fails_Validation()
        {
            var dto = new AddBasketItemDto
            {
                UserId = Guid.Empty,
                ProductId = Guid.NewGuid(),
                Quantity = 1
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void Quantity_Zero_Fails_Validation()
        {
            var dto = new AddBasketItemDto
            {
                UserId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 0
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.Contains("range")));
        }
    }
}