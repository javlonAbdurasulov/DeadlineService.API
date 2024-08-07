using DeadlineService.Application.Interfaces.Base;

namespace DeadlineService.Application.Interfaces.Repostitories
{
    public interface IUserRepository :
        ICreateRepository<User>,
        IDeleteRepository,
        IUpdateRepository<User>,
        IGetAllRepository<User>,
        IGetByIdRepository<User>
    {
        public Task<User> GetByEmail(string email);
        public Task<User> GetByUsernameAsync(string username);
    }
}
