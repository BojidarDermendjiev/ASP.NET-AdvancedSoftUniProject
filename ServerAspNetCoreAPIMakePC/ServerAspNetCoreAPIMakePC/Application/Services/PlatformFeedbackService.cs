namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;
    
    using Interfaces;
    using DTOs.Feedback;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Service class responsible for managing platform feedback operations.
    /// Provides business logic for creating, retrieving, updating, and deleting platform feedback entries.
    /// Utilizes the platform feedback repository for data access and AutoMapper for mapping between entities and DTOs.
    /// </summary>
    public class PlatformFeedbackService : IPlatformFeedbackService
    {
        private readonly IPlatformFeedbackRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformFeedbackService"/> class.
        /// </summary>
        /// <param name="repository">Repository for platform feedback data operations.</param>
        /// <param name="mapper">AutoMapper instance for mapping entities and DTOs.</param>
        public PlatformFeedbackService(IPlatformFeedbackRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Retrieves a platform feedback entry by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the feedback entry.</param>
        /// <returns>The feedback DTO if found; otherwise, null.</returns>
        public async Task<PlatformFeedbackDto?> GetByIdAsync(int id)
        {
            var feedback = await this._repository.GetByIdAsync(id);
            return feedback is null ? null : this._mapper.Map<PlatformFeedbackDto>(feedback);
        }

        /// <summary>
        /// Retrieves all platform feedback entries.
        /// </summary>
        /// <returns>A collection of feedback DTOs.</returns>
        public async Task<IEnumerable<PlatformFeedbackDto>> GetAllAsync()
        {
            var feedbacks = await this._repository.GetAllAsync();
            return feedbacks.Select(feedback => this._mapper.Map<PlatformFeedbackDto>(feedback));
        }

        /// <summary>
        /// Retrieves all feedback entries submitted by a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A collection of feedback DTOs for the specified user.</returns>
        public async Task<IEnumerable<PlatformFeedbackDto>> GetByUserIdAsync(Guid userId)
        {
            var feedbacks = await this._repository.GetByUserIdAsync(userId);
            return feedbacks.Select(feedback => this._mapper.Map<PlatformFeedbackDto>(feedback));
        }


        /// <summary>
        /// Creates a new platform feedback entry.
        /// </summary>
        /// <param name="dto">The DTO containing feedback data to create.</param>
        /// <returns>The created feedback DTO.</returns>
        public async Task<PlatformFeedbackDto> CreateAsync(CreatePlatformFeedbackDto dto)
        {
            var feedback = this._mapper.Map<Domain.Entities.PlatformFeedback>(dto);
            feedback.DateGiven = DateTime.UtcNow;
            await this._repository.AddAsync(feedback);
            return this._mapper.Map<PlatformFeedbackDto>(feedback);
        }

        /// <summary>
        /// Updates an existing platform feedback entry.
        /// </summary>
        /// <param name="dto">The DTO containing updated feedback data.</param>
        /// <returns>The updated feedback DTO.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the feedback entry does not exist.</exception>
        public async Task<PlatformFeedbackDto> UpdateAsync(UpdatePlatformFeedbackDto dto)
        {
            var existingFeedback = await this._repository.GetByIdAsync(dto.Id);
            if (existingFeedback is null)
            {
                throw new KeyNotFoundException(string.Format(FeedbackNotFound));
            }
            this._mapper.Map(dto, existingFeedback);
            await this._repository.UpdateAsync(existingFeedback);
            return this._mapper.Map<PlatformFeedbackDto>(existingFeedback);
        }

        /// <summary>
        /// Deletes a platform feedback entry by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the feedback entry to delete.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the feedback entry does not exist.</exception>
        public async Task DeleteAsync(int id)
        {
            var existingFeedback = await this._repository.GetByIdAsync(id);
            if (existingFeedback is null)
            {
                throw new KeyNotFoundException(string.Format(FeedbackNotFound));
            }
            await this._repository.DeleteAsync(id);
        }
    }
}
