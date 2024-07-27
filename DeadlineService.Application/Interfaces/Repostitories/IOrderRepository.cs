using DeadlineService.Application.Interfaces.Base;

namespace DeadlineService.Application.Interfaces.Repostitories
{
    public interface IOrderRepository :
        ICreateService<Order>,
        IGetAllService<Order>,
        IUpdateRepository<Order>,
        IDeleteService
    {
    }
}
