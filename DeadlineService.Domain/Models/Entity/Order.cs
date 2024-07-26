using DeadlineService.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.Entity
{
    public class Order
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description{ get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
        public OrderStatus OrderStatus{ get; set; }


        public int CommentId { get; set; }
        public Comment Comment{ get; set; }

        [ForeignKey("CreatedByUser")]
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }

        [ForeignKey("AssignedToUser")]
        public int? AssignedToUserId { get; set; }
        public User? AssignedToUser { get; set; }
    }
}
