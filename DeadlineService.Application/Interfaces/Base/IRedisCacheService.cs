using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Base
{
    public interface IRedisCacheService
    {
        Task<byte[]?> GetAsync(string key);
        Task SetAsync(string key, byte[] value);
        Task DeleteAsync(string key);
    }
}
