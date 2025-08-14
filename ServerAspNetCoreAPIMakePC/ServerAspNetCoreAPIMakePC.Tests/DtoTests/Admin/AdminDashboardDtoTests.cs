namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Admin
{
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Admin;

    public class AdminDashboardDtoTests
    {
        [Test]
        public void Can_Construct_And_Read_Properties()
        {
            var dto = new AdminDashboardDto
            {
                TotalUsers = 100,
                TotalOrders = 50,
                TotalProducts = 25
            };

            Assert.AreEqual(100, dto.TotalUsers);
            Assert.AreEqual(50, dto.TotalOrders);
            Assert.AreEqual(25, dto.TotalProducts);
        }
    }
}