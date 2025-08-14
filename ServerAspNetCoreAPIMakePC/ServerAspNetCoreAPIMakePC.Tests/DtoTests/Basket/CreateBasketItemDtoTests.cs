namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Basket
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Basket;
    public class CreateBasketItemDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new CreateBasketItemDto
            {
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
        public void Invalid_Quantity_Fails_Validation()
        {
            var dto = new CreateBasketItemDto
            {
                ProductId = Guid.NewGuid(),
                Quantity = -1 
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("quantity")));
        }
    }
}