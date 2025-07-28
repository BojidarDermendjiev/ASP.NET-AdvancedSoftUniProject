namespace ServerAspNetCoreAPIMakePC.Application.DTOs.User
{
    using System.ComponentModel.DataAnnotations;

    public class AuthenticateUserDto
    {
        /// <summary>
        /// The user's email address.
        /// </summary>
        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        public string Password { get; set; } = null!;
    }
}
