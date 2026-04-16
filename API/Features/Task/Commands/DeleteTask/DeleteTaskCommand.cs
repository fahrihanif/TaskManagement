using MediatR;

namespace API.Features.Task.Commands.DeleteTask;

public record DeleteTaskCommand(int Id) : IRequest<bool>;