namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Feedback
{
    using System;
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Feedback;
    public class PlatformFeedbackDtoTests
    {
        [Test]
        public void Can_Construct_And_Read_Properties()
        {
            var id = 1;
            var userId = Guid.NewGuid();
            var date = DateTime.UtcNow;

            var dto = new PlatformFeedbackDto
            {
                Id = id,
                UserId = userId,
                UserName = "Tester",
                Rating = 5,
                Comment = "Awesome!",
                DateGiven = date
            };

            Assert.AreEqual(id, dto.Id);
            Assert.AreEqual(userId, dto.UserId);
            Assert.AreEqual("Tester", dto.UserName);
            Assert.AreEqual(5, dto.Rating);
            Assert.AreEqual("Awesome!", dto.Comment);
            Assert.AreEqual(date, dto.DateGiven);
        }
    }
}