﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Enum
{
    public enum OrderStatus : byte
    {
        Free,
        Busy,
        Done
    }
}
