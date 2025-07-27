namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs;
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserDto dto);
        Task<UserDto?> AuthenticateUserAsync(string email, string password);
        Task<UserDto?> GetUserByIdAsync(Guid userId);
        Task UpdateUserAsync(Guid userId, UpdateUserDto dto);
        Task DeleteUserAsync(Guid id);
        Task ChangePasswordAsync(Guid id, string newPassword);
    }
}
