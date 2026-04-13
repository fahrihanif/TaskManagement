namespace API.Contracts;

public interface ITaskRepository
{
    IEnumerable<string> GetAll();
}