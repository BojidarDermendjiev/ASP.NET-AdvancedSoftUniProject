namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;
    
    using DTOs;
    using Interfaces;
    using Utilities;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;

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
            var existingUser = await this._userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException(string.Format(UserExistingEmailAddress, dto.Email));
            }
            if (dto.Password != dto.ConfirmPassword)
            {
                throw new InvalidOperationException(InvalidMatchingPassword);
            }
            if (string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            {
                throw new ArgumentException("Password and Confirm Password cannot be empty.");
            }

            byte[] passwordSalt;
            string passwordHash = PasswordHasher.HashPassword(dto.Password, out passwordSalt);

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userRepository.AddAsync(user);
        }

        public Task<UserDto?> AuthenticateUserAsync(string email, string password)
        {
            throw new NotImplementedException();
        }
    }
}
