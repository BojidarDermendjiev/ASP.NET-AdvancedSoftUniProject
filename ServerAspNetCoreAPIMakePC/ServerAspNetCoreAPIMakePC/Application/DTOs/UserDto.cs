namespace ServerAspNetCoreAPIMakePC.Application.DTOs
{
    public class UserDto
    {
        /// <summary>
        /// Unique identifier for the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User's email address.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// User's full name.
        /// </summary>
        public string FullName { get; set; } = null!;

        /// <summary>
        /// User's role in the system.
        /// </summary>
        public string Role { get; set; } = null!;
    }
}
