﻿using DeadlineService.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.DTOs.Comment
{
    public class PostCommentDTO
    {
        public int SellerId { get; set; }
        public int? FrilancerId { get; set; }
        public Stars Stars { get; set; }
        public string Text { get; set; }
    }
}
