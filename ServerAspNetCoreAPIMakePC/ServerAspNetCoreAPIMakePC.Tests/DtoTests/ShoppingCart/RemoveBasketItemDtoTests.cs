namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.ShoppingCart
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart;
    public class RemoveBasketItemDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new RemoveBasketItemDto
            {
                UserId = Guid.NewGuid(),
                ProductId = Guid.NewGuid()
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(results);
        }
    }
}