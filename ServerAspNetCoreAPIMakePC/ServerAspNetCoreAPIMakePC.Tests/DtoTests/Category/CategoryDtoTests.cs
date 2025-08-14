namespace ServerAspNetCoreAPIMakePC.Tests.Application.DTOs.Category
{
    using NUnit.Framework;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.Category;
    public class CategoryDtoTests
    {
        [Test]
        public void Can_Construct_And_Read_Properties()
        {
            var dto = new CategoryDto
            {
                Id = 1,
                Name = "Components",
                Description = "PC building components"
            };

            Assert.AreEqual(1, dto.Id);
            Assert.AreEqual("Components", dto.Name);
            Assert.AreEqual("PC building components", dto.Description);
        }
    }
}