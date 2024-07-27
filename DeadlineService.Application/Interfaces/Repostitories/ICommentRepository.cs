using DeadlineService.Application.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeadlineService.Application.Interfaces.Repostitories
{
    public interface ICommentRepository:
        ICreateRepository<Comment>,
        IGetByIdRepository<Comment>,
        IGetAllRepository<Comment>,
        IUpdateRepository<Comment>,
        IDeleteRepository
    {
    }
}
