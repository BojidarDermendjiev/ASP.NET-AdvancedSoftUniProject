namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Order
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Order;
    public class OrderDtoTests
    {
        [Test]
        public void Can_Construct_And_Read_Properties()
        {
            var id = 7;
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;
            var items = new List<OrderItemDto>
            {
                new OrderItemDto
                {
                    Id = 1,
                    ProductId = Guid.NewGuid(),
                    ProductName = "Product1",
                    Quantity = 2,
                    UnitPrice = 50m
                }
            };

            var dto = new OrderDto
            {
                Id = id,
                UserId = userId,
                UserName = "Test User",
                OrderDate = date,
                ShippingAddress = "Addr",
                PaymentStatus = "Paid",
                TotalPrice = 100m,
                Items = items
            };

            Assert.AreEqual(id, dto.Id);
            Assert.AreEqual(userId, dto.UserId);
            Assert.AreEqual("Test User", dto.UserName);
            Assert.AreEqual(date, dto.OrderDate);
            Assert.AreEqual("Addr", dto.ShippingAddress);
            Assert.AreEqual("Paid", dto.PaymentStatus);
            Assert.AreEqual(100m, dto.TotalPrice);
            Assert.AreEqual(1, dto.Items.Count);
            Assert.AreEqual("Product1", ((List<OrderItemDto>)dto.Items)[0].ProductName);
        }
    }
}