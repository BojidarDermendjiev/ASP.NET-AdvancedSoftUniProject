namespace ServerAspNetCoreAPIMakePC.Domain.Interfaces
{
    using Entities;

    public interface IReviewRepository
    {
        Task<Review?> GetByIdAsync(int id);
        Task<IEnumerable<Review>> GetAllAsync();
        Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId);
        Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId);
        Task AddAsync(Review review);
        Task UpdateAsync(Review review);
        Task DeleteAsync(int id);
    }
}
