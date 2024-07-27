using DeadlineService.Application.Interfaces.Base;

namespace DeadlineService.Application.Interfaces.Repostitories
{
    public interface IUserRepository :
        ICreateService<User>,
        IDeleteService,
        IUpdateRepository<User>,
        IGetAllService<User>,
        IGetByIdService<User>
    {
    }
}
