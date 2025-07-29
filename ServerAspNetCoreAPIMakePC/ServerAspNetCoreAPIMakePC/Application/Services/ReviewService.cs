namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;

    using Interfaces;
    using DTOs.Review;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMapper mapper)
        {
            this._reviewRepository = reviewRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves a review by its unique identifier.
        /// </summary>
        public async Task<ReviewDto?> GetByIdAsync(int id)
        {
            var review = await this._reviewRepository.GetByIdAsync(id);
            return review is null ? null : this._mapper.Map<ReviewDto>(review);

        }

        /// <summary>
        /// Retrieves all reviews.
        /// </summary>
        public async Task<IEnumerable<ReviewDto>> GetAllAsync()
        {
            var reviews = await this._reviewRepository.GetAllAsync();
            return this._mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        /// <summary>
        /// Retrieves all reviews for a specific product.
        /// </summary>
        public async Task<IEnumerable<ReviewDto>> GetByProductIdAsync(Guid productId)
        {
            var reviews = await this._reviewRepository.GetByProductIdAsync(productId);
            return this._mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        /// <summary>
        /// Retrieves all reviews written by a specific user.
        /// </summary>
        public async Task<IEnumerable<ReviewDto>> GetByUserIdAsync(Guid userId)
        {
          var reviews = await this._reviewRepository.GetByUserIdAsync(userId);
            return this._mapper.Map<IEnumerable<ReviewDto>>(reviews);
        }

        /// <summary>
        /// Creates a new review.
        /// </summary>
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
        public async Task DeleteAsync(int id)
        {
            await this._reviewRepository.DeleteAsync(id);
        }
    }
}
