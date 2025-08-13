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
        public const string UserNotFound = "User not found.";
        public const string UserPasswordChangeSuccessfully = "Password changed successfully.";

        // Brand-related error messages
        public const string InvalidBrandName = "Brand name must be between 2 and 100 characters.";
        public const string InvalidBrandDescription = "Brand description must be between 10 and 500 characters.";
        public const string InvalidBrandLogoUrl = "Brand logo URL must be a valid URL.";
        public const string InvalidBrandCannotBeNull = "Brand cannot be empty. Please select a valid brand.";
        public const string InvalidBrandNotFound = "Brand not found.";
        public const string IdMismatchUserFriendly = "The ID you provided does not match our records. Please check the ID and try again.";

        // Category-related error messages
        public const string InvalidCategoryName = "Category name must be between 2 and 100 characters.";
        public const string InvalidCategoryDescriptionName = "Category description must be between 50 and 500 characters.";
        public const string InvalidCategoryCannotBeNull = "Category cannot be null.";
        public const string CategoryNotFound = "Category not found.";
        public const string CategoryMismatch = "ID mismatch.";

        // Order-related error messages
        public const string InvalidShippingAddress = "Shipping address must be between 10 and 200 characters and contain a valid street, city, and postal code.";
        public const string InvalidPaymentStatus = "Payment status must be either 'Pending', 'Completed', or 'Failed'.";
        public const string InvalidOrder = "Order information is missing. Please provide the required order details.";
        public const string OrderNotFound = "Order not found.";
        public const string OrderMismatch = "The provided order ID does not match the expected value.";
        public const string OrderTotalPriceErrorMessage = "Total price must be positive.";
        public const string InvalidQuantity = "Quantity must be at least 1.";
        public const string InvalidUnitPrice = "Unit price must be positive.";


        // Product-related error messages
        public const string InvalidProductName = "Product name must be between 2 and 100 characters.";
        public const string InvalidProductType = "Product type must be between 2 and 50 characters and can only contain letters, numbers, and spaces.";
        public const string InvalidProductDescription = "Product description must be between 50 and 1000 characters.";
        public const string InvalidProductSpecs = "Product specifications must be between 10 and 500 characters.";
        public const string InvalidProductStock = "Product stock must be a non-negative integer.";
        public const string InvalidProductPrice = "Price must be positive.";
        public const string AlreadyExistingProduct = "$Product with name '{0}' already exists.";
        public const string ProductNotFound = "Product not found.";
        public const string InvalidProductImageURL = "ImageUrl must be a valid URL.";
        public const string InvalidProductBrand = "Product brand must be a valid brand and cannot be empty.";
        public const string ProductNotExist = "Product does not exist.";
        public const string ShoppingNotFound = "Shopping cart not found for user.";
        
        // Review-related error messages
        public const string InvalidReviewComment = "Review comment must be between 50 and 1000 characters.";
        public const string InvalidReviewRating = "Review rating must be between 1 and 5.";
        public const string EmptyReview = "Review cannot me empty!";
        public const string ReviewNotFound = "Review not found.";
        public const string ReviewInvalid = "Review is invalid. Please check the comment and rating requirements.";
        public const string ReviewIdMismatch = "The provided review ID does not match the expected value.";

        // Feedback-related error messages
        public const string InvalidFeedbackComment = "Feedback comment must be between 10 and 1000 characters.";
        public const string InvalidFeedbackRating = "Feedback rating must be between 1 and 5.";
        public const string EmptyFeedback = "Feedback cannot be empty!";
        public const string FeedbackNotFound = "Feedback not found.";
        public const string IdMismatch = "The provided ID does not match the expected value. Please check and try again.";

        // Basket-related error messages
        public const string InvalidBasketCannotBeNull = "Basket cannot be empty. Please select or create a basket to continue.";
        public const string InvalidBasketNotFound = "Basket not found.";
        public const string BasketMismatch = "The provided basket ID does not match the expected value.";
        public const string InvalidBasketQuantity = "Quantity must be at least 1.";

        // General error messages
        public const string InvalidGuidFormat = "The provided ID is not a valid GUID format.";
    }
}
