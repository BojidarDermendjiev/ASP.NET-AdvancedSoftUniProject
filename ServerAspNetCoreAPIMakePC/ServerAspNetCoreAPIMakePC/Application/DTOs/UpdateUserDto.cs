namespace ServerAspNetCoreAPIMakePC.Application.DTOs
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.UserValidationConstants;
    public class UpdateUserDto
    {
        /// <summary>
        /// The full name of the user.
        /// </summary>
        [Required]
        [StringLength(UserFullNameMaxLength, MinimumLength = UserFullNameMinLength, ErrorMessage = InvalidUserName)]
        public string FullName { get; set; } = null!;

        /// <summary>
        /// The email address of the user.
        /// </summary>
        [Required]
        [EmailAddress(ErrorMessage = InvalidEmail)]
        public string Email { get; set; } = null!;

        /// <summary>
        /// The role assigned to the user (e.g., "Admin", "User").
        /// </summary>
        [Required]
        [StringLength(UserRoleMaxLength, MinimumLength = UserRoleMinLength, ErrorMessage = InvalidUserRole)]
        public string Role { get; set; } = null!;

        /// <summary>
        /// The new password for the user (optional). If provided, the user's password will be updated.
        /// </summary>
        [StringLength(UserPasswordMaxLength, MinimumLength = UserPasswordMinLength, ErrorMessage = InvalidPassword)]
        [RegularExpression(UserPasswordRegexPattern, ErrorMessage = InvalidPassword)]
        public string? Password { get; set; }
    }
}
