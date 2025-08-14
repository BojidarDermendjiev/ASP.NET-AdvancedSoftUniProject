namespace ServerAspNetCoreAPIMakePC.Tests.Services
{
    using Moq;
    using AutoMapper;
    using NUnit.Framework;
    using Microsoft.Extensions.Options;

    using Domain.Enums;
    using Domain.Entities;
    using Domain.Interfaces;
    using Domain.ValueObjects;
    using Application.Services;
    using Application.Settings;
    using Application.Interfaces;
    using Application.DTOs.Brand;

    [TestFixture]
    public class BrandServiceTests
    {
        private Mock<IBrandRepository> _brandRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private BrandService _brandService;

        [SetUp]
        public void SetUp()
        {
            this._brandRepositoryMock = new Mock<IBrandRepository>();
            this._mapperMock = new Mock<IMapper>();
            this._brandService = new BrandService(this._brandRepositoryMock.Object, this._mapperMock.Object);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsBrandDto_IfFound()
        {
            var brand = new Brand { Id = 1 };
            var brandDto = new BrandDto { Id = 1 };

            this._brandRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(brand);
            this._mapperMock.Setup(m => m.Map<BrandDto>(brand)).Returns(brandDto);
            
            var result = await this._brandService.GetByIdAsync(1);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_IfNotFound()
        {
            this._brandRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Brand)null);

            var result = await this._brandService.GetByIdAsync(1);

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllBrandDtos()
        {
            var brands = new List<Brand> { new Brand { Id = 1 }, new Brand { Id = 2 } };
            var brandDtos = new List<BrandDto> { new BrandDto { Id = 1 }, new BrandDto { Id = 2 } };

            this._brandRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(brands);
            this._mapperMock.Setup(m => m.Map<BrandDto>(brands[0])).Returns(brandDtos[0]);
            this._mapperMock.Setup(m => m.Map<BrandDto>(brands[1])).Returns(brandDtos[1]);

            var result = (await this._brandService.GetAllAsync()).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
        }

        [Test]
        public async Task CreateAsync_CreatesAndReturnsBrandDto()
        {
            var createDto = new CreateBrandDto { Name = "BrandA" };
            var brand = new Brand { Id = 3 };
            var brandDto = new BrandDto { Id = 3 };

            this._mapperMock.Setup(m => m.Map<Brand>(createDto)).Returns(brand);
            this._brandRepositoryMock.Setup(r => r.AddAsync(brand)).Returns(Task.CompletedTask);
            this._mapperMock.Setup(m => m.Map<BrandDto>(brand)).Returns(brandDto);

            var result = await this._brandService.CreateAsync(createDto);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Id);
            _brandRepositoryMock.Verify(r => r.AddAsync(brand), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_Throws_IfBrandNotFound()
        {
            var updateDto = new UpdateBrandDto { Id = 10 };

            this._brandRepositoryMock.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync((Brand)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await this._brandService.UpdateAsync(updateDto));
        }

        [Test]
        public async Task UpdateAsync_UpdatesAndReturnsBrandDto()
        {
            var updateDto = new UpdateBrandDto { Id = 4 };
            var brand = new Brand { Id = 4 };
            var brandDto = new BrandDto { Id = 4 };

            this._brandRepositoryMock.Setup(r => r.GetByIdAsync(updateDto.Id)).ReturnsAsync(brand);
            this._mapperMock.Setup(m => m.Map(updateDto, brand));
            this._brandRepositoryMock.Setup(r => r.UpdateAsync(brand)).Returns(Task.CompletedTask);
            this._mapperMock.Setup(m => m.Map<BrandDto>(brand)).Returns(brandDto);

            var result = await this._brandService.UpdateAsync(updateDto);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Id);
            this._brandRepositoryMock.Verify(r => r.UpdateAsync(brand), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_Throws_IfBrandNotFound()
        {
            this._brandRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Brand)null);

            Assert.ThrowsAsync<KeyNotFoundException>(async () => await _brandService.DeleteAsync(17));
        }

        [Test]
        public async Task DeleteAsync_DeletesBrand_IfFound()
        {
            var brand = new Brand { Id = 8 };
            this._brandRepositoryMock.Setup(r => r.GetByIdAsync(brand.Id)).ReturnsAsync(brand);
            this._brandRepositoryMock.Setup(r => r.DeleteAsync(brand.Id)).Returns(Task.CompletedTask);

            await _brandService.DeleteAsync(brand.Id);

            this._brandRepositoryMock.Verify(r => r.DeleteAsync(brand.Id), Times.Once);
        }
    }
}