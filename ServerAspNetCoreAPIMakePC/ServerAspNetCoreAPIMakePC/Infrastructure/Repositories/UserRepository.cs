namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    public class UserRepository : IUserRepository
    {
        private readonly MakePCDbContext _context;

        public UserRepository(MakePCDbContext context)
            => this._context = context;

        public async Task AddAsync(User user)
        {
            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await this._context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await this._context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), InvalidUserType);
            }
            this._context.Users.Update(user);
            await this._context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = this._context.Users.Find(id);
            if (user == null)
            {
                throw new KeyNotFoundException(string.Format(UserNotFoundById, id));
            }
            this._context.Users.Remove(user);
            await this._context.SaveChangesAsync();
        }
    }
}
