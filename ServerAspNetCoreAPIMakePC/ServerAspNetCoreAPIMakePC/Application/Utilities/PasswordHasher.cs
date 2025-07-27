namespace ServerAspNetCoreAPIMakePC.Application.Utilities
{
    using System.Text;
    using System.Security.Cryptography;

    public class PasswordHasher
    {
        private const int KeySize = 64;
        private const int Iterations = 350000;
        private static readonly HashAlgorithmName HashAlgorithm = HashAlgorithmName.SHA512;

        public static string HashPassword(string password, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(KeySize);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                salt,
                Iterations,
                HashAlgorithm,
                KeySize);

            return Convert.ToHexString(hash);
        }

        public static bool VerifyPassword(string password, string storedHash, byte[] storedSalt)
        {
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                storedSalt,
                Iterations,
                HashAlgorithm,
                KeySize);

            var hashToCompareString = Convert.ToHexString(hashToCompare);

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(storedHash),
                Encoding.UTF8.GetBytes(hashToCompareString)
            );
        }
    }
}
