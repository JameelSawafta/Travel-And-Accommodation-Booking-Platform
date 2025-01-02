using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using TravelAndAccommodationBookingPlatform.Domain.Interfaces.Services;

namespace PasswordHashing;

public class Argon2PasswordService : IPasswordService
{
    private readonly IConfiguration _configuration;
    private readonly int _saltSize;
    
    public Argon2PasswordService(IConfiguration configuration)
    {
        _configuration = configuration;
        _saltSize = int.Parse(_configuration["Argon2PasswordHashing:SaltSize"]);
    }
    
    public byte[] GenerateSalt()
    {
        byte[] salt = new byte[_saltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    public string? GenerateHashedPassword(string password, byte[] salt)
    {
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        using (var hasher = new Argon2id(passwordBytes))
        {
            hasher.Salt = salt;
            hasher.MemorySize = Convert.ToInt32(_configuration["Argon2PasswordHashing:MemorySize"]);
            hasher.DegreeOfParallelism = Convert.ToInt32(_configuration["Argon2PasswordHashing:DegreeOfParallelism"]);
            hasher.Iterations = Convert.ToInt32(_configuration["Argon2PasswordHashing:Iterations"]);
            hasher.KnownSecret = Encoding.UTF8.GetBytes(_configuration["Argon2PasswordHashing:Secret"]);
            var hash = hasher.GetBytes(Convert.ToInt32(_configuration["Argon2PasswordHashing:HashSize"]));
            return Convert.ToBase64String(hash);
        }
    }

    public bool VerifyPassword(string userPassword, string hashedPassword, byte[] salt)
    {
        return GenerateHashedPassword(userPassword, salt) == hashedPassword;
    }
}