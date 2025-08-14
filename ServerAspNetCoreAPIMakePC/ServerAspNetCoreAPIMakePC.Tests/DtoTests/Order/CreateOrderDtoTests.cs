namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Order
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Order;
    public class CreateOrderDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new CreateOrderDto
            {
                UserId = Guid.NewGuid(),
                ShippingAddress = "123 Main Street",
                PaymentStatus = "Paid",
                TotalPrice = 100.50m,
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 2,
                        UnitPrice = 50.25m
                    }
                }
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
            Assert.IsEmpty(results);
        }

        [Test]
        public void Short_ShippingAddress_Fails_Validation()
        {
            var dto = new CreateOrderDto
            {
                UserId = Guid.NewGuid(),
                ShippingAddress = string.Empty, 
                PaymentStatus = "Paid",
                TotalPrice = 100.50m,
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 2,
                        UnitPrice = 50.25m
                    }
                }
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("address")));
        }

        [Test]
        public void Invalid_TotalPrice_Fails_Validation()
        {
            var dto = new CreateOrderDto
            {
                UserId = Guid.NewGuid(),
                ShippingAddress = "123 Main Street",
                PaymentStatus = "Paid",
                TotalPrice = -1, 
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 2,
                        UnitPrice = 50.25m
                    }
                }
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
            Assert.IsTrue(results.Exists(r => r.ErrorMessage != null && r.ErrorMessage.ToLower().Contains("total")));
        }
    }
}