using DeadlineService.Application.Interfaces.Services;
using System.Security.Cryptography;
using System.Text;

namespace DeadlineService.Application.Services.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public bool VerifyPassword(string hashedPassword, string verifyPassword)
        {
            var result = StringToHash(verifyPassword) == hashedPassword;
            return result;
        }

        public string StringToHash(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] passwordInBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha.ComputeHash(passwordInBytes);

                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
