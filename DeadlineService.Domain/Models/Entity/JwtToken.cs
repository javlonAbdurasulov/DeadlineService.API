using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Domain.Models.Entity
{
    public class JwtToken
    {
        public string AccessToken{ get; set; }
        public string RefreshToken{ get; set; }
    }
}
