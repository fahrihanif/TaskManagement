namespace API.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color { get; set; } = "#0078D4";

    // Navigation property
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}