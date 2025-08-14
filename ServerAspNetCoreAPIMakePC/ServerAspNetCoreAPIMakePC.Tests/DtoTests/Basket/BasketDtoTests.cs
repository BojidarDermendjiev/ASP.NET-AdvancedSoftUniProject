namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Basket
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Basket;
    public class BasketDtoTests
    {
        [Test]
        public void Can_Construct_And_Read_Properties()
        {
            var id = 111;
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var items = new List<BasketItemDto>
            {
                new BasketItemDto
                {
                    Id = 2,
                    ProductId = Guid.NewGuid(),
                    ProductName = "Prod X",
                    Quantity = 2
                }
            };

            var dto = new BasketDto
            {
                Id = id,
                UserId = userId,
                UserName = "Alice",
                DateCreated = date,
                Items = items
            };

            Assert.AreEqual(id, dto.Id);
            Assert.AreEqual(userId, dto.UserId);
            Assert.AreEqual("Alice", dto.UserName);
            Assert.AreEqual(date, dto.DateCreated);
            Assert.AreEqual(1, dto.Items.Count);
            Assert.AreEqual("Prod X", ((List<BasketItemDto>)dto.Items)[0].ProductName);
        }
    }
}