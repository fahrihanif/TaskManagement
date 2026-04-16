using API.Contracts;
using MediatR;

namespace API.Features.Task.Commands.DeleteTask;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly IUnitOfWork _uow;

    public DeleteTaskCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<bool> Handle(
        DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _uow.Tasks.GetByIdAsync(request.Id);
        if (task is null) return false;

        await _uow.Tasks.DeleteAsync(task);
        await _uow.SaveChangesAsync();

        return true;
    }
}