using API.Contracts;
using API.DTOs;
using API.Entities;
using API.Mappings;
using MediatR;

namespace API.Features.Task.Commands.CreateTask;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskResponseDto>
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<CreateTaskCommandHandler> _logger;

    public CreateTaskCommandHandler(
        IUnitOfWork uow,
        ILogger<CreateTaskCommandHandler> logger)
    {
        _uow    = uow;
        _logger = logger;
    }
    
    public async Task<TaskResponseDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
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

        _logger.LogInformation(
            "Task created: {TaskId} - {Title}", task.Id, task.Title);

        return task.ToDto();

    }
}