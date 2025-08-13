namespace ServerAspNetCoreAPIMakePC.Application.DTOs.User
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.UserValidationConstants;

    public class ChangePasswordDto
    {
        /// <summary>
        /// The old (current) password of the user.
        /// </summary>
        [Required(ErrorMessage = "Current password is required.")]
        [StringLength(UserPasswordMaxLength, MinimumLength = UserPasswordMinLength, ErrorMessage = InvalidPassword)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = null!;

        /// <summary>
        /// The new password for the user.
        /// </summary>
        [Required(ErrorMessage = "New password is required.")]
        [StringLength(UserPasswordMaxLength, MinimumLength = UserPasswordMinLength, ErrorMessage = InvalidPassword)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;
    }
}
