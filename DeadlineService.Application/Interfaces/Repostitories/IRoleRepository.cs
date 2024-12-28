using DeadlineService.Application.Interfaces.Base;
using DeadlineService.Domain.Models.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Repostitories
{
    public interface IRoleRepository:
       ICreateRepository<Role>,
        IGetAllRepository<Role>
    {
        public Task<IEnumerable<Role>> GetAllWithUserAsync();
    }
}
