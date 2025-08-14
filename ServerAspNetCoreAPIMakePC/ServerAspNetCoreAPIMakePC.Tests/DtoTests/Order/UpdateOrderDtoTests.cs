namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Order
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Order;

    public class UpdateOrderDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new UpdateOrderDto
            {
                Id = 8,
                UserId = Guid.NewGuid(),
                ShippingAddress = "456 New Street",
                PaymentStatus = "Refunded",
                TotalPrice = 99.99m,
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 1,
                        UnitPrice = 99.99m
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
        public void Invalid_ShippingAddress_Fails_Validation()
        {
            var dto = new UpdateOrderDto
            {
                Id = 8,
                UserId = Guid.NewGuid(),
                ShippingAddress = string.Empty,
                PaymentStatus = "Paid",
                TotalPrice = 20,
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 1,
                        UnitPrice = 20
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
            var dto = new UpdateOrderDto
            {
                Id = 8,
                UserId = Guid.NewGuid(),
                ShippingAddress = "Addr",
                PaymentStatus = "Paid",
                TotalPrice = -10,
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 1,
                        UnitPrice = 20
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