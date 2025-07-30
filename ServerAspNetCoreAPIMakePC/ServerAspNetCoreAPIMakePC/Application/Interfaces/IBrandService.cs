namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs.Brand;

    public interface IBrandService
    {
        Task<BrandDto?> GetByIdAsync(int id);
        Task<IEnumerable<BrandDto>> GetAllAsync();
        Task<BrandDto> CreateAsync(CreateBrandDto dto);
        Task<BrandDto> UpdateAsync(UpdateBrandDto dto);
        Task DeleteAsync(int id);
    }
}
