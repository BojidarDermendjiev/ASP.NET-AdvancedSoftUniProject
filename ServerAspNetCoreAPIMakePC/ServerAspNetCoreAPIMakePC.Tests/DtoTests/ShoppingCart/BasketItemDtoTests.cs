namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.ShoppingCart
{
    using System;
    using NUnit.Framework;
    
    using ServerAspNetCoreAPIMakePC.Application.DTOs.ShoppingCart;
    public class BasketItemDtoTests
    {
        [Test]
        public void Can_Construct_And_Read_Properties()
        {
            var id = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var dto = new BasketItemDto
            {
                Id = id,
                ProductId = productId,
                ProductName = "Product",
                Quantity = 3,
                Price = 9.99m
            };

            Assert.AreEqual(id, dto.Id);
            Assert.AreEqual(productId, dto.ProductId);
            Assert.AreEqual("Product", dto.ProductName);
            Assert.AreEqual(3, dto.Quantity);
            Assert.AreEqual(9.99m, dto.Price);
        }
    }
}