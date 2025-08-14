namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Basket
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Basket;

    public class CreateBasketDtoTests
    {
        [Test]
        public void Valid_Model_Passes_Validation()
        {
            var dto = new CreateBasketDto
            {
                UserId = Guid.NewGuid(),
                Items = new List<CreateBasketItemDto>
                {
                    new CreateBasketItemDto
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 2
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
        public void Missing_UserId_Fails_Validation()
        {
            var dto = new CreateBasketDto
            {
                Items = new List<CreateBasketItemDto>
                {
                    new CreateBasketItemDto
                    {
                        ProductId = Guid.NewGuid(),
                        Quantity = 2
                    }
                }
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsTrue(isValid);
        }

        [Test]
        public void Missing_Items_Fails_Validation()
        {
            var dto = new CreateBasketDto
            {
                UserId = Guid.NewGuid(),
                Items = null
            };

            var ctx = new ValidationContext(dto);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(dto, ctx, results, true);

            Assert.IsFalse(isValid);
        }
    }
}