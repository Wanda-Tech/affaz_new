using System;
using System.Linq;
using System.Security.Cryptography;

public class PasswordGenerator
{
    private static readonly char[] _chars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=[]{}|;:,.<>?".ToCharArray();

    public static string Generate(int length = 12)
    {
        if (length <= 0) throw new ArgumentException("Password length must be greater than zero.");

        using var rng = RandomNumberGenerator.Create();
        var result = new char[length];
        var buffer = new byte[sizeof(uint)];

        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(buffer);
            uint num = BitConverter.ToUInt32(buffer, 0);
            result[i] = _chars[num % _chars.Length];
        }

        return new string(result);
    }
}
