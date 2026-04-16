using API.Features.Task.Commands.CreateTask;
using API.Features.Task.Commands.DeleteTask;
using API.Features.Task.Queries.GetAllTasks;
using API.Features.Task.Queries.GetTaskById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{ 
    private readonly ISender _sender;
    
    public TasksController(ISender sender)
        => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetAllTasksQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _sender.Send(new GetTaskByIdQuery(id));
        return result is null
            ? NotFound(new { message = $"Task {id} not found" })
            : Ok(result);
    } 

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskCommand request)
    {
        var result = await _sender.Send(request);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _sender.Send(new DeleteTaskCommand(id));
        return deleted
            ? NoContent()
            : NotFound(new { message = $"Task {id} not found" });
    }
}
