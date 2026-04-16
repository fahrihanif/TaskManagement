using API.DTOs;
using MediatR;

namespace API.Features.Task.Commands.CreateTask;

public record CreateTaskCommand(
    string Title,
    string? Description,
    string Priority,
    DateTime? DueDate,
    string? AssigneeEmail,
    int CategoryId
) : IRequest<TaskResponseDto>;