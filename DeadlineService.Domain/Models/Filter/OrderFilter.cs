using DeadlineService.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.Filter
{
    public class OrderFilter
    {
        public string? TitlePart { get; set; }
        public string? Category { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Deadline { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
