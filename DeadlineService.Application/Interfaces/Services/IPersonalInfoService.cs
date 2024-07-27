using DeadlineService.Application.Interfaces.Base;
using DeadlineService.Domain.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Services
{
    public interface IPersonalInfoService:
        ICreateService<PersonalInfo>,
        IGetAllService<PersonalInfo>,
        IGetByIdService<PersonalInfo>,
        IUpdateService<PersonalInfo>,
        IDeleteService
    {

    }
}
