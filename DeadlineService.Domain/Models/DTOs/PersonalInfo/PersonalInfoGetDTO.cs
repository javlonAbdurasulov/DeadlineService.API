using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.DTOs.PersonalInfo
{
    public class PersonalInfoGetDTO
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public byte[]? Photo { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CreateAt { get; set; }

        public int UserId { get; set; }
    }
}
