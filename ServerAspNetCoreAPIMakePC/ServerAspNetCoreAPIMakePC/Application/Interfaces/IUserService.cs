namespace ServerAspNetCoreAPIMakePC.Application.Interfaces
{
    using DTOs;
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserDto dto);
        Task<UserDto?> AuthenticateUserAsync(string email, string password);
    }
}
