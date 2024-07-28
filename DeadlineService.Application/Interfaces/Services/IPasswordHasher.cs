using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Services
{
    public interface IPasswordHasher
    {
        public string StringToHash(string password);
        public bool VerifyPassword(string hashedPasswordword,string verifyPassword);
    }
}
