using API.Contracts;
using API.Data;

namespace API.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public ITaskRepository Tasks { get; }
    public ICategoryRepository Categories { get; }

    public UnitOfWork(
        AppDbContext context,
        ITaskRepository tasks,
        ICategoryRepository categories)
    {
        _context   = context;
        Tasks      = tasks;
        Categories = categories;
    }

    public async Task<int> SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}