namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs.Category;

    public interface ICategoryService
    {
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
        Task<CategoryDto> UpdateAsync(UpdateCategoryDto dto);
        Task DeleteAsync(int id);
    }
}
