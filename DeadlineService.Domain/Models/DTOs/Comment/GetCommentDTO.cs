using DeadlineService.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.DTOs.Comment
{
    public class GetCommentDTO
    {
        public int? Id { get; set; }
        public int? SellerId { get; set; }
        public int? FrilancerId { get; set; }
        public Stars Stars { get; set; }
        public string Text { get; set; }
    }
}
