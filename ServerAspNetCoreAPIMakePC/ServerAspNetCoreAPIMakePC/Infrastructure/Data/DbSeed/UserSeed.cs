namespace ServerAspNetCoreAPIMakePC.Infrastructure.Data.DbSeed
{
    using Domain.Entities;
    using Domain.ValueObjects;
    public static class UserSeed
    {
        public static List<User> GetUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = new Email("user@example.com"),
                    PasswordHash = "hashedpassword",
                    ConfirmPassword = "hashedpassword",
                    PasswordSalt = new byte[] { 0x20, 0x21, 0x22, 0x23 },
                    FullName = new FullName("John Doe"),
                    Role = "User",
                    AvatarImage = new byte[] { 0x01, 0x02, 0x03, 0x04 },
                    AvatarUrl = "/images/avatars/user.png",
                    ShoppingCart = new ShoppingCart()
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Email = new Email("admin@example.com"),
                    PasswordHash = "adminhashedpassword",
                    ConfirmPassword = "adminhashedpassword",
                    PasswordSalt = new byte[] { 0x30, 0x31, 0x32, 0x33 },
                    FullName = new FullName("Admin User"),
                    Role = "Admin",
                    AvatarImage = new byte[] { 0x05, 0x06, 0x07, 0x08 },
                    AvatarUrl = "/images/avatars/admin.png",
                    ShoppingCart = new ShoppingCart()
                }
            };
        }
    }
}