using API.DTOs;
using API.Entities;

namespace API.Mappings;

public static class TaskMapper
{
    public static TaskResponseDto ToDto(this TaskItem t) => new(
        t.Id,
        t.Title,
        t.Description,
        t.Status.ToString(),
        t.Priority.ToString(),
        t.DueDate,
        t.AssigneeEmail,
        t.CreatedAt,
        t.Category?.Name ?? "Unknown"
    );
}