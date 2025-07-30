namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs.Feedback;

    public interface IPlatformFeedbackService
    {
        Task<PlatformFeedbackDto?> GetByIdAsync(int id);
        Task<IEnumerable<PlatformFeedbackDto>> GetAllAsync();
        Task<IEnumerable<PlatformFeedbackDto>> GetByUserIdAsync(Guid userId);
        Task<PlatformFeedbackDto> CreateAsync(CreatePlatformFeedbackDto dto);
        Task<PlatformFeedbackDto> UpdateAsync(UpdatePlatformFeedbackDto dto);
        Task DeleteAsync(int id);
    }
}
