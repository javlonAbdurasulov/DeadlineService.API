using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.Entity
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name  { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
