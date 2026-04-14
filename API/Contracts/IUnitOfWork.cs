namespace API.Contracts;

public interface IUnitOfWork : IDisposable
{
    ITaskRepository Tasks { get; }
    ICategoryRepository Categories { get; }
    Task<int> SaveChangesAsync();
}