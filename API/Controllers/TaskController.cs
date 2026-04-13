using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskRepository _taskRepository;

    public TasksController(ITaskRepository taskRepository)
        => _taskRepository = taskRepository;

    [HttpGet]
    public IActionResult GetAll()
    {
        var tasks = _taskRepository.GetAll();
        
        return Ok(tasks);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        if (id == 99)
            throw new KeyNotFoundException($"Task with ID {id} was not found.");
        
        
        return Ok(new
        {
            id,
            message = $"Task {id} endpoint is working!"
        });
    }
}
