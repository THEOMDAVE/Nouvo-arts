using System.Security.Cryptography;
using System.Text;

namespace NouvoStudio.Utilities
{
    /// <summary>
    /// Utility class to generate password hashes for admin configuration.
    /// Run this in a console app or use the method directly to generate hashes.
    /// </summary>
    public static class PasswordHashGenerator
    {
        /// <summary>
        /// Generates a hash for the given password.
        /// Use this to generate the PasswordHash value for appsettings.json
        /// </summary>
        public static string GenerateHash(string password)
        {
            return PasswordHasher.HashPassword(password);
        }

        /// <summary>
        /// Example usage - can be called from a console app or test
        /// </summary>
        public static void Example()
        {
            var password = "admin123!";
            var hash = GenerateHash(password);
            Console.WriteLine($"Password: {password}");
            Console.WriteLine($"Hash: {hash}");
            Console.WriteLine($"Use this hash in appsettings.json: PasswordHash = \"{hash}\"");
        }
    }
}

