using API.DTOs;
using MediatR;

namespace API.Features.Task.Queries.GetAllTasks;

public record GetAllTasksQuery : IRequest<IEnumerable<TaskResponseDto>>;