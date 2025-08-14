namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Basket
{
    using System;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Basket;
    public class BasketItemDtoTests
    {
        [Test]
        public void Can_Construct_And_Read_Properties()
        {
            var id = 5;
            var productId = Guid.NewGuid();

            var dto = new BasketItemDto
            {
                Id = id,
                ProductId = productId,
                ProductName = "Widget",
                Quantity = 3
            };

            Assert.AreEqual(id, dto.Id);
            Assert.AreEqual(productId, dto.ProductId);
            Assert.AreEqual("Widget", dto.ProductName);
            Assert.AreEqual(3, dto.Quantity);
        }
    }
}