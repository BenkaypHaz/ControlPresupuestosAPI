using System.Security.Cryptography;
using System.Text;

public class Security
{
    public static string HashPassword(string password, out string salt)
    {
        salt = GenerateSalt();
        var hash = HashPasswordWithSalt(password, salt);
        return $"{salt}:{hash}";
    }

    private static string GenerateSalt()
    {
        byte[] randomBytes = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes);
    }

    public static string HashPasswordWithSalt(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = string.Concat(password, salt);
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return hash;
        }
    }
}
