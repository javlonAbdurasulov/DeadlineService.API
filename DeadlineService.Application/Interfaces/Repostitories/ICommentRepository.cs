using DeadlineService.Application.Interfaces.Base;

namespace DeadlineService.Application.Interfaces.Repostitories
{
    public interface ICommentRepository :
        ICreateRepository<Comment>,
        IGetByIdRepository<Comment>,
        IGetAllRepository<Comment>,
        IUpdateRepository<Comment>,
        IDeleteRepository
    {
    }
}
