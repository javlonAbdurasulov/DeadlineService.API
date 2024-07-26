using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.Entity
{
    public class User
    {
        public User(int id, string username, string passwordHash)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;

        }
        public int Id{ get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public int? PersonalInfoId{ get; set; }
        public PersonalInfo? PersonalInfo{ get; set; }
        public List<Order>? Orders{ get; set; }


    }
}
