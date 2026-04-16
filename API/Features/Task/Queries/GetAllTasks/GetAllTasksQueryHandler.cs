using API.Contracts;
using API.DTOs;
using API.Mappings;
using MediatR;

namespace API.Features.Task.Queries.GetAllTasks;

public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskResponseDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllTasksQueryHandler(IUnitOfWork uow) => _uow = uow;
    
    public async Task<IEnumerable<TaskResponseDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _uow.Tasks.GetAllAsync();
        return tasks.Select(t => t.ToDto());
    }
}