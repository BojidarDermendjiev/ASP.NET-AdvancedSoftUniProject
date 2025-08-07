namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;
    
    /// <summary>
    /// Repository implementation for managing PlatformFeedback entities in the database.
    /// Provides methods for CRUD operations and retrieval of platform feedbacks, including eager loading of related user data.
    /// </summary>
    public class PlatformFeedbackRepository : IPlatformFeedbackRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformFeedbackRepository"/> class.
        /// </summary>
        /// <param name="context">The database context for performing platform feedback operations.</param>
        /// 
        private readonly MakePCDbContext _context;

        public PlatformFeedbackRepository(MakePCDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Retrieves a platform feedback by its unique identifier, including the associated user.
        /// </summary>
        /// <param name="id">The unique identifier of the platform feedback.</param>
        /// <returns>The platform feedback entity if found; otherwise, null.</returns>
        public async Task<PlatformFeedback?> GetByIdAsync(int id)
        {
            return await this._context.PlatformFeedbacks
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        /// <summary>
        /// Retrieves all platform feedbacks from the database, including their associated user data.
        /// </summary>
        /// <returns>A collection of platform feedback entities.</returns>
        public async Task<IEnumerable<PlatformFeedback>> GetAllAsync()
        {
            return await this._context.PlatformFeedbacks
                .Include(f => f.User)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all platform feedbacks associated with a specific user, including the user data.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A collection of platform feedback entities for the specified user.</returns>
        public async Task<IEnumerable<PlatformFeedback>> GetByUserIdAsync(Guid userId)
        {
            return await this._context.PlatformFeedbacks
                .Include(f => f.User)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new platform feedback entity to the database.
        /// </summary>
        /// <param name="feedback">The platform feedback entity to add.</param>
        /// <exception cref="ArgumentNullException">Thrown if the feedback is null.</exception>
        public async Task AddAsync(PlatformFeedback feedback)
        {
            if (feedback is null)
            {
                throw new ArgumentNullException(nameof(feedback), string.Format(EmptyFeedback));
            }
            await this._context.PlatformFeedbacks.AddAsync(feedback);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing platform feedback entity in the database.
        /// </summary>
        /// <param name="feedback">The platform feedback entity with updated values.</param>
        public async Task UpdateAsync(PlatformFeedback feedback)
        {
            this._context.PlatformFeedbacks.Update(feedback);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a platform feedback entity from the database by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the platform feedback to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the platform feedback does not exist.</exception>
        public async Task DeleteAsync(int id)
        {
            var feedback = await this.GetByIdAsync(id);
            if (feedback is null)
            {
                throw new KeyNotFoundException(string.Format(FeedbackNotFound));
            }
            this._context.PlatformFeedbacks.Remove(feedback);
            await this._context.SaveChangesAsync();
        }
    }
}
