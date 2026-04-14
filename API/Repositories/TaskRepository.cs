using API.Contracts;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories;

public class TaskRepository : Repository<TaskItem>, ITaskRepository
{
    public TaskRepository(AppDbContext context) : base(context) { }
    
    public override async Task<IEnumerable<TaskItem>> GetAllAsync()
        => await _context.Tasks
                         .Include(t => t.Category)
                         .ToListAsync();

    public override async Task<TaskItem?> GetByIdAsync(int id)
        => await _context.Tasks
                         .Include(t => t.Category)
                         .FirstOrDefaultAsync(t => t.Id == id);
}