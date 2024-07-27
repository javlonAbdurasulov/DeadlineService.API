using DeadlineService.Application.Interfaces.Base;
using DeadlineService.Domain.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Repostitories
{
    public interface IPersonalInfoRepository:
        ICreateRepository<PersonalInfo>,
        IGetByIdRepository<PersonalInfo>,
        IGetAllRepository<PersonalInfo>,
        IUpdateRepository<PersonalInfo>,
        IDeleteRepository

    {
    }
}
