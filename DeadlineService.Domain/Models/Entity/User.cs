using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.Entity
{
    public class User
    {
        public User(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;

        }
        public int Id{ get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

        public int? PersonalInfoId{ get; set; }
        public PersonalInfo? PersonalInfo{ get; set; }

        public ICollection<Role>? Roles { get; set; }
        public ICollection<Order>? CreatedOrders { get; set; }
        public ICollection<Order>? AssignedOrders { get; set; }


    }
}
