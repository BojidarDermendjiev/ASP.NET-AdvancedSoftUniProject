
namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Brand
{
    using NUnit.Framework;

    using ServerAspNetCoreAPIMakePC.Application.DTOs.Brand;
    public class BrandDtoTests
    {
        [Test]
        public void Can_Construct_And_Read_Properties()
        {
            var dto = new BrandDto
            {
                Id = 1,
                Name = "TestBrand",
                Description = "Test brand description",
                LogoUrl = "https://example.com/logo.png"
            };

            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual("TestBrand", dto.Name);
            Assert.AreEqual("Test brand description", dto.Description);
            Assert.AreEqual("https://example.com/logo.png", dto.LogoUrl);
        }
    }
}