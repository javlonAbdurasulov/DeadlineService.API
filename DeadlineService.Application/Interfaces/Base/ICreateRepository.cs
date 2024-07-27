namespace DeadlineService.Application.Interfaces.Base
{
    public interface ICreateRepository<T> where T : class
    {
    public Task<T> CreateAsync(T obj);
    }
}
