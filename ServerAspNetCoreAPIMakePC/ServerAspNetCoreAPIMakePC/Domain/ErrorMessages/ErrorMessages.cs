namespace ServerAspNetCoreAPIMakePC.Domain.ErrorMessages
{
    public class ErrorMessages
    {
        // User-related error messages
        public const string InvalidPassword = "Password must have at least 1 uppercase, 1 lowercase, 1 number, and 1 special character.";
        public const string InvalidEmail = "Email is required.";
        public const string InvalidMatchingPassword = "The password and confirmation password do not match.";
        public const string InvalidUserName = "Username must be between 2 and 100 characters and can only contain letters, numbers, and underscores.";
        public const string InvalidUserRole = "User role must be between 3 and 30 characters and can only contain letters, numbers, and underscores.";
        public const string InvalidUserType = "User cannot be null";
        public const string UserNotFoundById = "User with ID {0} not found.";
        public const string UserExistingEmailAddress = "User with email {0} already exists";
        public const string EmptyUserPassword = "Password and Confirm Password cannot be empty.";

        // Brand-related error messages
        public const string InvalidBrandName = "Brand name must be between 2 and 100 characters.";
        public const string InvalidBrandDescription = "Brand description must be between 10 and 500 characters.";
        public const string InvalidBrandLogoUrl = "Brand logo URL must be a valid URL.";

        // Category-related error messages
        public const string InvalidCategoryName = "Category name must be between 2 and 100 characters.";
        public const string InvalidCategoryDescriptionName = "Category description must be between 50 and 500 characters.";

        // Order-related error messages
        public const string InvalidShippingAddress = "Shipping address must be between 10 and 200 characters and contain a valid street, city, and postal code.";
        public const string InvalidPaymentStatus = "Payment status must be either 'Pending', 'Completed', or 'Failed'.";

        // Product-related error messages
        public const string InvalidProductName = "Product name must be between 2 and 100 characters.";
        public const string InvalidProductType = "Product type must be between 2 and 50 characters and can only contain letters, numbers, and spaces.";
        public const string InvalidProductDescription = "Product description must be between 50 and 1000 characters.";
        public const string InvalidProductSpecs = "Product specifications must be between 10 and 500 characters.";
        public const string InvalidProductStock = "Product stock must be a non-negative integer.";
        public const string InvalidProductPrice = "Price must be positive.";

        // Review-related error messages
        public const string InvalidReviewComment = "Review comment must be between 50 and 1000 characters.";
        public const string InvalidReviewRating = "Review rating must be between 1 and 5.";

        // Feedback-related error messages
        public const string InvalidFeedbackComment = "Feedback comment must be between 10 and 1000 characters.";
        public const string InvalidFeedbackRating = "Feedback rating must be between 1 and 5.";
    }
}
