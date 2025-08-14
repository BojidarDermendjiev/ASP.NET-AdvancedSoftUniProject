namespace ServerAspNetCoreAPIMakePC.Tests.Utilities
{
    using NUnit.Framework;
    using ServerAspNetCoreAPIMakePC.Application.Utilities;

    public class PasswordHasherTests
    {
        [Test]
        public void HashPassword_And_VerifyPassword_Works()
        {
            var password = "TestPassword!123";

            var hash = PasswordHasher.HashPassword(password, out var salt);

            Assert.IsNotNull(hash);
            Assert.IsNotNull(salt);
            Assert.IsTrue(PasswordHasher.VerifyPassword(password, hash, salt));
        }

        [Test]
        public void VerifyPassword_With_Wrong_Password_Fails()
        {
            var hash = PasswordHasher.HashPassword("RightPassword", out var salt);

            Assert.IsFalse(PasswordHasher.VerifyPassword("WrongPassword", hash, salt));
        }
    }
}
