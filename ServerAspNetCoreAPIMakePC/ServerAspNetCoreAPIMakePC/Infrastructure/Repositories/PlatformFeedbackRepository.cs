namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;
    public class PlatformFeedbackRepository : IPlatformFeedbackRepository
    {
        private readonly MakePCDbContext _context;

        public PlatformFeedbackRepository(MakePCDbContext context)
        {
            this._context = context;
        }
        public async Task<PlatformFeedback?> GetByIdAsync(int id)
        {
            return await this._context.PlatformFeedbacks
                .Include(f => f.User)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<PlatformFeedback>> GetAllAsync()
        {
            return await this._context.PlatformFeedbacks
                .Include(f => f.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlatformFeedback>> GetByUserIdAsync(Guid userId)
        {
            return await this._context.PlatformFeedbacks
                .Include(f => f.User)
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(PlatformFeedback feedback)
        {
            if (feedback is null)
            {
                throw new ArgumentNullException(nameof(feedback), string.Format(EmptyFeedback));
            }
            await this._context.PlatformFeedbacks.AddAsync(feedback);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PlatformFeedback feedback)
        {
            this._context.PlatformFeedbacks.Update(feedback);
            await this._context.SaveChangesAsync();
        }

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
