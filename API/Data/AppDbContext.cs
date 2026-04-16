using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
      public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
      {
            
      }

    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureCategory(modelBuilder);
        ConfigureTaskItem(modelBuilder);
        SeedData(modelBuilder);
    }

    private static void ConfigureCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(c => c.Color)
                  .HasMaxLength(7)
                  .HasDefaultValue("#0078D4");
        });
    }

    private static void ConfigureTaskItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(t => t.Description)
                  .HasMaxLength(1000);

            entity.Property(t => t.AssigneeEmail)
                  .HasMaxLength(256);

            // Store enums as strings for readability
            entity.Property(t => t.Status)
                  .HasConversion<string>();

            entity.Property(t => t.Priority)
                  .HasConversion<string>();

            // Default CreatedAt to database UTC time
            entity.Property(t => t.CreatedAt)
                  .HasDefaultValueSql("GETUTCDATE()");

            // Relationship: TaskItem belongs to Category
            entity.HasOne(t => t.Category)
                  .WithMany(c => c.Tasks)
                  .HasForeignKey(t => t.CategoryId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
    
    private static void SeedData(ModelBuilder modelBuilder)
    {
          modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Development",   Color = "#0078D4" },
                new Category { Id = 2, Name = "Testing",       Color = "#107C10" },
                new Category { Id = 3, Name = "Documentation", Color = "#8764B8" }
          );

          modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                      Id = 1, Title = "Design database schema",
                      Status = TaskItemStatus.Done, Priority = TaskPriority.High,
                      CategoryId = 1, CreatedAt = DateTime.UtcNow
                },
                new TaskItem
                {
                      Id = 2, Title = "Implement API endpoints",
                      Status = TaskItemStatus.InProgress, Priority = TaskPriority.High,
                      CategoryId = 1, CreatedAt = DateTime.UtcNow
                },
                new TaskItem
                {
                      Id = 3, Title = "Write unit tests",
                      Status = TaskItemStatus.Todo, Priority = TaskPriority.Medium,
                      CategoryId = 2, CreatedAt = DateTime.UtcNow
                }
          );
    }
}