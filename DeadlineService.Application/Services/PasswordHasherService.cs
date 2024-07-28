using DeadlineService.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace DeadlineService.Application.Services
{
    public class PasswordHasherService : IPasswordHasher
    {
        public bool VerifyPassword(string hashedPassword, string verifyPassword)
        {
            var result = StringToHash(verifyPassword) ==hashedPassword;
            return result;
        }

        public string StringToHash(string password)
        {
            using (SHA256 sha = SHA256.Create()) {

                byte[] passwordInBytes=Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = sha.ComputeHash(passwordInBytes);

                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
