namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;
    
    using DTOs;
    using Utilities;
    using Interfaces;
    using Domain.Entities;
    using Domain.Interfaces;
    using static Domain.ErrorMessages.ErrorMessages;
    using ServerAspNetCoreAPIMakePC.Application.DTOs.User;

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this._userRepository = userRepository;
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
                throw new ArgumentException(EmptyUserPassword);
            }

            byte[] passwordSalt;
            string passwordHash = PasswordHasher.HashPassword(dto.Password, out passwordSalt);

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userRepository.AddAsync(user);
        }

        public async Task<UserDto?> AuthenticateUserAsync(string email, string password)
        {
            var user = await this._userRepository.GetByEmailAsync(email);
            if (user is null)
            {
                return null;
            }

            bool verified = PasswordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);

            if (!verified)
            {
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user is null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user is null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task UpdateUserAsync(Guid userId, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                throw new InvalidOperationException(string.Format(UserNotFoundById, userId));
            }

            _mapper.Map(dto, user);

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                byte[] passwordSalt;
                string passwordHash = PasswordHasher.HashPassword(dto.Password, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null;
        }
        public async Task ChangePasswordAsync(Guid id, string newPassword)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user is null)
            {
                throw new KeyNotFoundException(string.Format(UserNotFoundById, id));
            }

            byte[] passwordSalt;
            string passwordHash = PasswordHasher.HashPassword(newPassword, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _userRepository.UpdateAsync(user);
        }
    }
}
