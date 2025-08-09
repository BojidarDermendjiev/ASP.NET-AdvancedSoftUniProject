namespace ServerAspNetCoreAPIMakePC.Application.Services
{
    using AutoMapper;
    using System.Text;
    using System.Security.Claims;
    using Microsoft.Extensions.Options;
    using System.IdentityModel.Tokens.Jwt;
    using Microsoft.IdentityModel.Tokens;


    using Settings;
    using DTOs.User;
    using Utilities;
    using Interfaces;
    using Domain.Entities;
    using Domain.Interfaces;
    using Domain.ValueObjects;

    using static Domain.ErrorMessages.ErrorMessages;

    /// <summary>
    /// Service class responsible for user management logic, including registration, authentication,
    /// profile updates, password management, and user retrieval.
    /// Utilizes the user repository for data persistence and AutoMapper for DTO-entity mapping.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">Repository for user-related database operations.</param>
        /// <param name="mapper">AutoMapper instance for DTO-entity mapping.</param>
        public UserService(IUserRepository userRepository, IMapper mapper, IOptions<JwtSettings> jwtOptions)
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
            this._jwtSettings = jwtOptions.Value;
        }

        /// <summary>
        /// Registers a new user if the email does not already exist and the passwords match.
        /// </summary>
        /// <param name="dto">The user registration data.</param>
        /// <exception cref="InvalidOperationException">Thrown if the email is already registered or passwords do not match.</exception>
        /// <exception cref="ArgumentException">Thrown if the passwords are empty.</exception>
        public async Task RegisterUserAsync(RegisterUserDto dto)
        {
            var existingUser = await this._userRepository.GetByEmailAsync(new Email(dto.Email));
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

        /// <summary>
        /// Authenticates a user by email and password.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The authenticated user as a UserDto, or null if authentication fails.</returns>
        public async Task<UserDto?> AuthenticateUserAsync(Email email, string password)
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

        /// <summary>
        /// Gets a user by their unique identifier.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <returns>The user as a UserDto, or null if not found.</returns>
        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user is null ? null : _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Gets a user by their email address.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <returns>The user as a UserDto, or null if not found.</returns>
        public async Task<UserDto?> GetUserByEmailAsync(Email email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user is null ? null : _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Updates user profile information and password if provided.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <param name="dto">The updated profile data.</param>
        /// <exception cref="InvalidOperationException">Thrown if the user does not exist.</exception>
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

        /// <summary>
        /// Deletes a user by their unique identifier.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        public async Task DeleteUserAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }

        /// <summary>
        /// Checks if an email address is already registered.
        /// </summary>
        /// <param name="email">The email address to check.</param>
        /// <returns>True if the email exists, false otherwise.</returns>
        public async Task<bool> EmailExistsAsync(Email email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user != null;
        }

        /// <summary>
        /// Changes the user's password to the specified new password.
        /// </summary>
        /// <param name="id">The user's unique identifier.</param>
        /// <param name="newPassword">The new password.</param>
        /// <exception cref="KeyNotFoundException">Thrown if the user does not exist.</exception>
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

        public string GenerateJwtToken(UserDto userDto)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userDto.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userDto.Email),
                new Claim(ClaimTypes.Name, userDto.FullName ?? ""),
                // new Claim(ClaimTypes.Role, userDto.Role)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.LifespanMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
