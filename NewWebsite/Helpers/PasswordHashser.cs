using System;
using System.Security.Cryptography;
using System.Text;

namespace NewWebsite.Helpers;

public class PasswordHasher
{
    // Hash the password using MD5
    public static string HashPassword(string password)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert to hexadecimal string
            StringBuilder sb = new StringBuilder();
            foreach (var b in hashBytes)
                sb.Append(b.ToString("x2")); // Lowercase hex

            return sb.ToString();
        }
    }

    // Verify that a password matches the hash
    public static bool VerifyPassword(string inputPassword, string storedHash)
    {
        string hashOfInput = HashPassword(inputPassword);
        return StringComparer.OrdinalIgnoreCase.Compare(hashOfInput, storedHash) == 0;
    }
}
