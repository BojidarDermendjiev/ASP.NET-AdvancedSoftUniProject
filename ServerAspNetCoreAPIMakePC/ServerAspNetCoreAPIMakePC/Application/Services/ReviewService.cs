namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;

    using Interfaces;
    using DTOs.Review;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Service class responsible for handling business logic related to product reviews.
    /// Provides methods to create, retrieve, update, and delete reviews, as well as fetch reviews by product or user.
    /// Utilizes AutoMapper for mapping between entities and DTOs, and the review repository for data persistence.
    /// </summary>
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReviewService"/> class.
        /// </summary>
        /// <param name="reviewRepository">Repository for review data operations.</param>
        /// <param name="mapper">AutoMapper instance for mapping between entities and DTOs.</param>
        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            this._reviewRepository = reviewRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves a review by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the review.</param>
        /// <returns>The corresponding review DTO, or null if not found.</returns>
        public async Task<ReviewDto?> GetByIdAsync(int id)
        {
            var review = await this._reviewRepository.GetByIdAsync(id);
            return review is null ? null : this._mapper.Map<ReviewDto>(review);

        }

        /// <summary>
        /// Retrieves all reviews.
        /// </summary>
        /// <returns>A collection of all review DTOs.</returns>
        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            var reviews = await this._reviewRepository.GetAllAsync();
            return this._mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        /// <summary>
        /// Retrieves all reviews for a specific product.
        /// </summary>
        /// <param name="productId">The unique identifier of the product.</param>
        /// <returns>A collection of review DTOs for the specified product.</returns>
        public async Task<IEnumerable<ReviewDto>> GetByProductIdAsync(Guid productId)
        {
            var reviews = await this._reviewRepository.GetByProductIdAsync(productId);
            return this._mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        /// <summary>
        /// Retrieves all reviews written by a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A collection of review DTOs written by the specified user.</returns>
        public async Task<IEnumerable<ReviewDto>> GetByUserIdAsync(Guid userId)
        {
          var reviews = await this._reviewRepository.GetByUserIdAsync(userId);
            return this._mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        /// <summary>
        /// Creates a new review.
        /// </summary>
        /// <param name="dto">The DTO containing data for the new review.</param>
        /// <returns>The created review as a DTO.</returns>
        public async Task<ReviewDto> CreateAsync(CreateReviewDto dto)
        {
            var review = this._mapper.Map<Review>(dto);
            review.Date = DateTime.UtcNow;
            await this._reviewRepository.AddAsync(review);
            return this._mapper.Map<ReviewDto>(review);
        }

        /// <summary>
        /// Updates an existing review.
        /// </summary>
        /// <param name="dto">The DTO containing updated review data.</param>
        /// <returns>The updated review as a DTO.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the review does not exist.</exception>
        public async Task<ReviewDto> UpdateAsync(UpdateReviewDto dto)
        {
            var existingReview = await this._reviewRepository.GetByIdAsync(dto.Id);
            if (existingReview is null)
            {
                throw new KeyNotFoundException(string.Format(ReviewNotFound));
            }
            this._mapper.Map(dto, existingReview);
            await this._reviewRepository.UpdateAsync(existingReview);
            return this._mapper.Map<ReviewDto>(existingReview);
        }

        /// <summary>
        /// Deletes a review by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the review to delete.</param>
        public async Task DeleteAsync(int id)
        {
            await this._reviewRepository.DeleteAsync(id);
        }
    }
}
