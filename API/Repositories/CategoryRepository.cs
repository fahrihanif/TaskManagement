using API.Contracts;
using API.Data;
using API.Entities;

namespace API.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context) { }
}