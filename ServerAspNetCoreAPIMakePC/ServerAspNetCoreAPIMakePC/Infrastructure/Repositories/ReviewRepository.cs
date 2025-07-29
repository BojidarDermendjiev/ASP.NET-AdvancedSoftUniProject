namespace ServerAspNetCoreAPIMakePC.Infrastructure.Repositories
{
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;


    /// <summary>
    /// Repository implementation for managing Review entities in the database.
    /// Provides CRUD operations and methods for retrieving reviews by user or product.
    /// </summary>
    public class ReviewRepository : IReviewRepository
    {
        private readonly MakePCDbContext _context;
        private readonly IMapper _mapper;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewRepository"/> class.
        /// </summary>
        public ReviewRepository(MakePCDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves a review by its unique identifier, including related user and product.
        /// </summary>
        public async Task<Review?> GetByIdAsync(int id)
        {
            return await this._context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Retrieves all reviews from the database, including related users and products.
        /// </summary>
        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await this._context.Reviews
                .Include(r => r.User)
                .Include(r => r.Product)
                .ToListAsync();
        }


        /// <summary>
        /// Retrieves all reviews for a specific product.
        /// </summary>
        public async Task<IEnumerable<Review>> GetByProductIdAsync(Guid productId)
        {
            return await this._context.Reviews
                .Where(r => r.ProductId == productId)
                .Include(r => r.User)
                .Include(r => r.Product)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves all reviews written by a specific user.
        /// </summary>
        public async Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId)
        {
            return await this._context.Reviews
                .Where(r => r.UserId == userId)
                .Include(r => r.User)
                .Include(r => r.Product)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new review to the database and sets its date to the current UTC time.
        /// </summary>
        public async Task AddAsync(Review review)
        {
            if (review is null)
            {
                throw new ArgumentNullException(nameof(review), string.Format(EmptyReview));
            }
            review.Date = DateTime.UtcNow;
            await this._context.Reviews.AddAsync(review);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing review's properties and sets its date to the current UTC time.
        /// </summary>
        public async Task UpdateAsync(Review review)
        {
            if (review is null)
            {
                throw new ArgumentNullException(nameof(review), string.Format(EmptyReview));
            }
            var existingReview = await this.GetByIdAsync(review.Id);
            if (existingReview is null)
            {
                throw new KeyNotFoundException(string.Format(ReviewNotFound));
            }
            this._mapper.Map(review, existingReview);
            existingReview.Date = DateTime.UtcNow;
            this._context.Reviews.Update(existingReview);
            await this._context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a review by its unique identifier.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var review = await this.GetByIdAsync(id);
            if (review is null)
            {
                throw new KeyNotFoundException(string.Format(ReviewNotFound));
            }
            this._context.Reviews.Remove(review);
            await this._context.SaveChangesAsync();
        }
    }
}
