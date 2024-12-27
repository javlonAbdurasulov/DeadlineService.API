using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.Entity
{
    public class PersonalInfo
    {
        public PersonalInfo() { }
        public PersonalInfo(string email,string phoneNumber,int userId,string? description, byte[]? photo)
        {
            CreateAt = DateTime.Now;

            Email = email;
            PhoneNumber = phoneNumber;
            Description = description;
            UserId = userId;
            Photo = photo;

        }
        public int Id { get; set; }
        public string? Description { get; set; }
        public byte[]? Photo { get; set; }
        public string? Email { get; set; }
        public bool isEmailConfirmed{ get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreateAt { get; set; }

        public int UserId{ get; set; }
        public User User{ get; set; }
    }
}
