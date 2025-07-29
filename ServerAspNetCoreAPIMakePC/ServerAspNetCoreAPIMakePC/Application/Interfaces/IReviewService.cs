namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs.Review;

    public interface IReviewService
    {
        Task<ReviewDto?> GetByIdAsync(int id);
        Task<IEnumerable<ReviewDto>> GetAllAsync();
        Task<IEnumerable<ReviewDto>> GetByProductIdAsync(Guid productId);
        Task<IEnumerable<ReviewDto>> GetByUserIdAsync(Guid userId);
        Task<ReviewDto> CreateAsync(CreateReviewDto dto);
        Task<ReviewDto> UpdateAsync(UpdateReviewDto dto);
        Task DeleteAsync(int id);
    }
}
