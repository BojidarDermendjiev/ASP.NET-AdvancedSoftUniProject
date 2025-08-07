namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;
    using Domain.ValueObjects;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Repository implementation for managing User entities in the database.
    /// Provides CRUD operations for users.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly MakePCDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        public UserRepository(MakePCDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        public async Task AddAsync(User user)
        {
            await this._context.Users.AddAsync(user);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        public async Task<User?> GetByEmailAsync(Email email)
        {
            return await this._context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await this._context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        /// <summary>
        /// Updates the specified user entity in the database.
        /// </summary>
        public async Task UpdateAsync(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user), InvalidUserType);
            }
            this._context.Users.Update(user);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            var user = await this._context.Users.FindAsync(id);
            if (user is null)
            {
                throw new KeyNotFoundException(string.Format(UserNotFoundById, id));
            }
            this._context.Users.Remove(user);
            await this._context.SaveChangesAsync();
        }
    }
}
