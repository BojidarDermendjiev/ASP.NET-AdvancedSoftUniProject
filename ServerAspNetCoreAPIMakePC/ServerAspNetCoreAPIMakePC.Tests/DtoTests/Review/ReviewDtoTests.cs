namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Review
{
    using System;
    using NUnit.Framework;
    
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Review;
    public class ReviewDtoTests
    {
        [Test]
        public void Can_Construct_And_Read_Properties()
        {
            var id = 123;
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var date = DateTime.UtcNow;

            var dto = new ReviewDto
            {
                Id = id,
                UserId = userId,
                ProductId = productId,
                Rating = 3,
                Comment = "Nice product",
                Date = date,
                UserName = "Alice",
                ProductName = "ProductX"
            };

            Assert.AreEqual(id, dto.Id);
            Assert.AreEqual(userId, dto.UserId);
            Assert.AreEqual(productId, dto.ProductId);
            Assert.AreEqual(3, dto.Rating);
            Assert.AreEqual("Nice product", dto.Comment);
            Assert.AreEqual(date, dto.Date);
            Assert.AreEqual("Alice", dto.UserName);
            Assert.AreEqual("ProductX", dto.ProductName);
        }
    }
}