using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.Entity
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text{ get; set; }
        public byte Stars{ get; set; }

        public int OrderId{ get; set; }
        public Order Order{ get; set; }
    }
}
