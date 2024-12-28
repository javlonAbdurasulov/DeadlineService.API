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
            CreateAt = DateTime.Now.ToLongDateString() ;
            Email = email;
            PhoneNumber = phoneNumber;
            Description = description;
            UserId = userId;
            Photo = photo;

        }
        [Key]
        public int Id { get; set; }
        public string? Description { get; set; }
        public byte[]? Photo { get; set; }
        public string? Email { get; set; }
        public bool isEmailConfirmed { get; set; } = false;
        public string? PhoneNumber { get; set; }
        public string CreateAt { get; set; }=DateTime.Now.ToString();

        public int UserId{ get; set; }
        public User User{ get; set; }
    }
}
