namespace DeadlineService.Application.Interfaces.Base
{
    public interface ICreateService<T> where T : class
    {
        public abstract Task<bool> DeleteAsync(T obj);
    }
}
