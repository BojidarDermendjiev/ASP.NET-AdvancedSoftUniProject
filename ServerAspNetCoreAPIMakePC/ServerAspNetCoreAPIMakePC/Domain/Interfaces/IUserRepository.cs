namespace ServerAspNetCoreAPIMakePC.Domain.Interfaces
{
    using Entities;

    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
