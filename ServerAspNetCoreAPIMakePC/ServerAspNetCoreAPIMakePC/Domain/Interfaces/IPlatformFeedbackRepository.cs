namespace ServerAspNetCoreAPIMakePC.Domain.Interfaces
{
    using Entities;

    public interface IPlatformFeedbackRepository
    {
        Task<PlatformFeedback?> GetByIdAsync(int id);
        Task<IEnumerable<PlatformFeedback>> GetAllAsync();
        Task<IEnumerable<PlatformFeedback>> GetByUserIdAsync(Guid userId);
        Task AddAsync(PlatformFeedback feedback);
        Task UpdateAsync(PlatformFeedback feedback);
        Task DeleteAsync(int id);
    }
}
