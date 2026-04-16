using API.Contracts;
using API.DTOs;
using API.Mappings;
using MediatR;

namespace API.Features.Task.Queries.GetTaskById;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskResponseDto>
{
    private readonly IUnitOfWork _uow;

    public GetTaskByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<TaskResponseDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _uow.Tasks.GetByIdAsync(request.Id);
        return task?.ToDto();
    }
}