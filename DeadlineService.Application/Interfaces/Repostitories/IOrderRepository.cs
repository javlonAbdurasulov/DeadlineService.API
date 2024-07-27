using DeadlineService.Application.Interfaces.Base;

namespace DeadlineService.Application.Interfaces.Repostitories
{
    public interface IOrderRepository :
        ICreateRepository<Order>,
        IGetAllRepository<Order>,
        IUpdateRepository<Order>,
        IDeleteRepository
    {
    }
}
