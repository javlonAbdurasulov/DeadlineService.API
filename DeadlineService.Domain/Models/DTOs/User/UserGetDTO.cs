using DeadlineService.Domain.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.DTOs.User
{
    public class UserGetDTO
    {
        public int Id{ get; set; }
        public string Username { get; set; }
        public int? PersonalInfoId { get; set; }

        //public ICollection<Order>? CreatedOrders { get; set; }
        //public ICollection<Order>? AssignedOrders { get; set; }
    }
}
