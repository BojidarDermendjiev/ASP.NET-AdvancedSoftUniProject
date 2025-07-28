namespace ServerAspNetCoreAPIMakePC.Application.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class LoginDto
    {
        /// <summary>
        /// The user's email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        /// <summary>
        /// The user's password.
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
