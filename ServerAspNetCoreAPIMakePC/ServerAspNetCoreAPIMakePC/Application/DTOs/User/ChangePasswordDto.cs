namespace ServerAspNetCoreAPIMakePC.Application.DTOs.User
{
    using System.ComponentModel.DataAnnotations;

    using static Domain.ErrorMessages.ErrorMessages;
    using static Domain.Constants.UserValidationConstants;

    public class ChangePasswordDto
    {
        /// <summary>
        /// The new password for the user.
        /// </summary>
        [Required]
        [StringLength(UserPasswordMaxLength, MinimumLength = UserPasswordMinLength, ErrorMessage = InvalidPassword)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;
    }
}
