namespace API.DTOs;

public record TaskResponseDto(
    int Id,
    string Title,
    string? Description,
    string Status,
    string Priority,
    DateTime? DueDate,
    string? AssigneeEmail,
    DateTime CreatedAt,
    string CategoryName
);
