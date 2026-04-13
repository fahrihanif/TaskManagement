using API.Contracts;

namespace API.Repositories;

public class TaskRepository : ITaskRepository
{
    public IEnumerable<string> GetAll()
    {
        return new List<string>
        {
            "Task 1: Design database schema",
            "Task 2: Implement API endpoints",
            "Task 3: Write unit tests"
        };
    }
}