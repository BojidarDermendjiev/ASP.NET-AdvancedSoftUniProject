namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs.User;
    using Domain.ValueObjects;


    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserDto dto);
        Task<UserDto?> AuthenticateUserAsync(Email email, string password);
        Task<UserDto?> GetUserByIdAsync(Guid userId);
        Task UpdateUserAsync(Guid userId, UpdateUserDto dto);
        Task DeleteUserAsync(Guid id);
        Task ChangePasswordAsync(Guid id, string newPassword);
        string GenerateJwtToken(UserDto userDto);
        public Task<IEnumerable<UserDto>> GetClientsAsync();
    }
}
