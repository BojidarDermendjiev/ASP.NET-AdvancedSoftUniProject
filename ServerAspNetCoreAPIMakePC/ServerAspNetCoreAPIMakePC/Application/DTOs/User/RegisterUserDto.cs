namespace ServerAspNetCoreAPIMakePC.Application.DTOs.User
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.UserValidationConstants;

    public class RegisterUserDto
    {
        /// <summary>
        /// The user's email address.
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(UserEmailMaxLength, MinimumLength = UserEmailMinLength, ErrorMessage = InvalidEmail)]
        public string Email { get; set; } = null!;

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        [StringLength(UserPasswordMaxLength, MinimumLength = UserPasswordMinLength, ErrorMessage = InvalidPassword)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        /// <summary>
        /// The password confirmation (must match Password).
        /// </summary>
        [Required]
        [StringLength(UserConfirmPasswordMaxLength, MinimumLength = UserConfirmPasswordMinLength, ErrorMessage = InvalidMatchingPassword)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = InvalidMatchingPassword)]
        public string ConfirmPassword { get; set; } = null!;

        /// <summary>
        /// The user's full name.
        /// </summary>
        [Required]
        [StringLength(UserFullNameMaxLength, MinimumLength = UserFullNameMinLength, ErrorMessage = InvalidUserName)]
        public string FullName { get; set; } = null!;
    }
}
