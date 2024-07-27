using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Base
{
    public interface IDeleteService
    {
        public abstract Task<bool> DeleteAsync(int id);
    }
}
