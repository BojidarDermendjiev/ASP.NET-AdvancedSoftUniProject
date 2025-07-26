namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;
    
    using DTOs;
    using Interfaces;
    using Utilities;
    using Domain.Entities;
    using Domain.Interfaces;
    
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository  = userRepository;
            this._mapper = mapper;
        }   
        public async Task RegisterUserAsync(RegisterUserDto dto)
        {
            byte[] passwordSalt;
            string passwordHash = PasswordHasher.HashPassword(dto.Password, out passwordSalt);
            var user = _mapper.Map<User>(dto);
            await this._userRepository.AddAsync(user);
        }

        public Task<UserDto?> AuthenticateUserAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
