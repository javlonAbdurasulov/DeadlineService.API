using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Base
{
    public interface IRedisCacheService<T>
    {
        Task<byte[]> GetAsync(string key,T value);
        Task SetAsync(string key, byte[] value);
    }
}
