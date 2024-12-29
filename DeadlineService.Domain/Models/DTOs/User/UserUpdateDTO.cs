using DeadlineService.Domain.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.DTOs.User
{
    public class UserUpdateDTO
    {
        public int UserId { get; set; }
        public string? Description { get; set; }
        public byte[]? Photo { get; } = null;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTimeUtc { get; set; } = DateTime.UtcNow;
    }
}
