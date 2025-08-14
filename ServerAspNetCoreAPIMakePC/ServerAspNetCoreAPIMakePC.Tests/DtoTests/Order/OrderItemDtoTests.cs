namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Order
{
    using System;
    using NUnit.Framework;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Order;
    public class OrderItemDtoTests
    {
        [Test]
        public void Can_Construct_And_Read_Properties()
        {
            var id = 9;
            var productId = Guid.NewGuid();

            var dto = new OrderItemDto
            {
                Id = id,
                ProductId = productId,
                ProductName = "Prod",
                Quantity = 3,
                UnitPrice = 12.5m
            };

            Assert.AreEqual(id, dto.Id);
            Assert.AreEqual(productId, dto.ProductId);
            Assert.AreEqual("Prod", dto.ProductName);
            Assert.AreEqual(3, dto.Quantity);
            Assert.AreEqual(12.5m, dto.UnitPrice);
        }
    }
}