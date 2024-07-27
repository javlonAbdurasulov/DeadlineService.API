using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.DTOs.User
{
    public class LoginUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
