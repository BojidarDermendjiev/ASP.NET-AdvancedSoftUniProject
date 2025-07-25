namespace ServerAspNetCoreAPIMakePC.Domain.Entities
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    using static ErrorMessages.ErrorMessages;
    using static Constants.UserValidationConstants;
    public class User
    {
        [Required]
        [Comment("Unique identifier for the user.")]
        public Guid Id { get; set; }

        [Required]
        [Comment("Email address of the user.")]
        [StringLength(UserEmailMaxLength, MinimumLength = UserEmailMinLength)]
        [RegularExpression(UserEmailRegexPattern, ErrorMessage = InvalidEmail)]
        public string Email { get; set; } = null!;

        [Required]
        [Comment("Password hash for the user.")]
        [StringLength(UserPasswordMaxLength, MinimumLength = UserPasswordMinLength)]
        [RegularExpression(UserPasswordRegexPattern, ErrorMessage = InvalidPassword)]
        public string PasswordHash { get; set; } = null!;

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [StringLength(UserConfirmPasswordMaxLength, MinimumLength = UserConfirmPasswordMinLength)]
        [Compare(nameof(ConfirmPassword), ErrorMessage = InvalidMatchingPassword)]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [Comment("Full name of the user.")]
        [StringLength(UserFullNameMaxLength, MinimumLength = UserFullNameMinLength , ErrorMessage = InvalidUsername)]
        public string FullName { get; set; } = null!;

        [Required]
        [Comment("Role of the user in the system (e.g., Admin, User).")]
        [StringLength(UserRoleMaxLength, MinimumLength = UserRoleMinLength , ErrorMessage = InvalidUserRole)]
        public string Role { get; set; } = null!;

        [Required]
        [Comment("Navigation property to the shopping cart associated with the user.")]
        public ShoppingCart ShoppingCart { get; set; } = null!;

        [Comment("Collection of orders placed by the user.")]
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        [Comment("Collection of reviews written by the user.")]
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
