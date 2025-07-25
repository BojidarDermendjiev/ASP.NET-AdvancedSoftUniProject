namespace ServerAspNetCoreAPIMakePC.Domain.Constants
{
    public class UserValidationConstants
    {
        public const int UserEmailMaxLength = 100;
        public const int UserEmailMinLength = 5;
        public const string UserEmailRegexPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public const string UserPasswordRegexPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
        public const int UserPasswordMinLength = 8;
        public const int UserPasswordMaxLength = 100;

        public const int UserFullNameMaxLength = 100;
        public const int UserFullNameMinLength = 2;

        public const int UserRoleMaxLength = 30;
        public const int UserRoleMinLength = 3;

        public const int UserConfirmPasswordMinLength = 8;
        public const int UserConfirmPasswordMaxLength = 100;
    }
}
