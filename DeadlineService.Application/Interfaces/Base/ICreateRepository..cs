namespace DeadlineService.Application.Interfaces.Base
{
    public interface ICreateService<T> where T : class
    {
    public Task<T> CreateAsync(T obj);
    }
}
