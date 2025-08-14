namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.ShoppingCart
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart;

    public class ShoppingCartDtoTests
    {
        [Test]
        public void Can_Construct_And_Read_Properties()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var items = new List<BasketItemDto>
            {
                new BasketItemDto
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product1",
                    Quantity = 1,
                    Price = 10.5m
                }
            };

            var dto = new ShoppingCartDto
            {
                Id = id,
                UserId = userId,
                DateCreated = date,
                Items = items
            };

            Assert.AreEqual(id, dto.Id);
            Assert.AreEqual(userId, dto.UserId);
            Assert.AreEqual(date, dto.DateCreated);
            Assert.AreEqual(1, dto.Items.Count);
            Assert.AreEqual("Product1", ((List<BasketItemDto>)dto.Items)[0].ProductName);
        }
    }
}