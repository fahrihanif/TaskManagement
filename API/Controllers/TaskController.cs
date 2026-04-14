using API.Contracts;
using API.Entities;
using API.Mappings;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IUnitOfWork _uow;

    public TasksController(IUnitOfWork uow) => _uow = uow;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _uow.Tasks.GetAllAsync();
        return Ok(tasks.Select(t => t.ToDto()));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var task = await _uow.Tasks.GetByIdAsync(id);
        return task is null
            ? NotFound(new { message = $"Task {id} not found" })
            : Ok(task.ToDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request)
    {
        var task = new TaskItem
        {
            Title         = request.Title,
            Description   = request.Description,
            Priority      = Enum.Parse<TaskPriority>(request.Priority, true),
            DueDate       = request.DueDate,
            AssigneeEmail = request.AssigneeEmail,
            CategoryId    = request.CategoryId
        };

        await _uow.Tasks.AddAsync(task);
        await _uow.SaveChangesAsync();

        // Reload with Category included
        task = (await _uow.Tasks.GetByIdAsync(task.Id))!;

        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task.ToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _uow.Tasks.GetByIdAsync(id);
        if (task is null) return NotFound(new { message = $"Task {id} not found" });

        await _uow.Tasks.DeleteAsync(task);
        await _uow.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateTaskRequest request)
    {
        var task = await _uow.Tasks.GetByIdAsync(id);
        if (task is null) return NotFound(new { message = $"Task {id} not found" });
        
        task.Title         = request.Title;
        task.Description   = request.Description;
        task.Priority      = Enum.Parse<TaskPriority>(request.Priority, true);
        task.DueDate       = request.DueDate;
        task.AssigneeEmail = request.AssigneeEmail;
        task.CategoryId    = request.CategoryId;
        
        await _uow.Tasks.UpdateAsync(task);
        await _uow.SaveChangesAsync();
        return NoContent();
    }
}

// Inline request model — will be replaced by CreateTaskCommand in Lab 6
public record CreateTaskRequest(
    string Title,
    string? Description,
    string Priority,
    DateTime? DueDate,
    string? AssigneeEmail,
    int CategoryId
);
