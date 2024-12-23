using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Base
{
    public interface IGetByIdRepository<T> where T : class
    {
        public Task<T?> GetByIdAsync(int id);
    }
}
