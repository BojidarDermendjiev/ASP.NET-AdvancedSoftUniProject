namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Order
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Order;
    public class CreateOrderItemDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new CreateOrderItemDto
            {
                ProductId = Guid.NewGuid(),
                Quantity = 3,
                UnitPrice = 15.25m
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
            var dto = new CreateOrderItemDto
            {
                ProductId = Guid.NewGuid(),
                Quantity = 0, 
                UnitPrice = 15.25m
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("quantity")));
        }

        [Test]
        public void Invalid_UnitPrice_Fails_Validation()
        {
            var dto = new CreateOrderItemDto
            {
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                UnitPrice = -100m 
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("unit price")));
        }
    }
}