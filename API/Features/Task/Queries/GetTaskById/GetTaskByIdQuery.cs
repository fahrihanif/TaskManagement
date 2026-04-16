using API.DTOs;
using MediatR;

namespace API.Features.Task.Queries.GetTaskById;

public record GetTaskByIdQuery(int Id) : IRequest<TaskResponseDto>;