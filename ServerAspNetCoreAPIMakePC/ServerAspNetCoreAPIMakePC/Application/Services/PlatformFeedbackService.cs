namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;
    
    using Interfaces;
    using DTOs.Feedback;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;
    public class PlatformFeedbackService : IPlatformFeedbackService
    {
        private readonly IPlatformFeedbackRepository _repository;
        private readonly IMapper _mapper;

        public PlatformFeedbackService(IPlatformFeedbackRepository repository, IMapper mapper)
        {
            this._repository = repository;
            this._mapper = mapper;
        }
        public async Task<PlatformFeedbackDto?> GetByIdAsync(int id)
        {
            var feedback = await this._repository.GetByIdAsync(id);
            return feedback is null ? null : this._mapper.Map<PlatformFeedbackDto>(feedback);
        }

        public async Task<IEnumerable<PlatformFeedbackDto>> GetAllAsync()
        {
            var feedbacks = await this._repository.GetAllAsync();
            return feedbacks.Select(feedback => this._mapper.Map<PlatformFeedbackDto>(feedback));
        }

        public async Task<IEnumerable<PlatformFeedbackDto>> GetByUserIdAsync(Guid userId)
        {
            var feedbacks = await this._repository.GetByUserIdAsync(userId);
            return feedbacks.Select(feedback => this._mapper.Map<PlatformFeedbackDto>(feedback));
        }

        public async Task<PlatformFeedbackDto> CreateAsync(CreatePlatformFeedbackDto dto)
        {
            var feedback = this._mapper.Map<Domain.Entities.PlatformFeedback>(dto);
            feedback.DateGiven = DateTime.UtcNow;
            await this._repository.AddAsync(feedback);
            return this._mapper.Map<PlatformFeedbackDto>(feedback);
        }

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
