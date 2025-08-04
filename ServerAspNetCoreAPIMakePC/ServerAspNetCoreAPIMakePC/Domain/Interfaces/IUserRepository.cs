namespace ServerAspNetCoreAPIMakePC.Domain.Interfaces
{
    using ValueObjects;

    using Entities;

    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByEmailAsync(Email email);
        Task<User?> GetByIdAsync(Guid id);
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
