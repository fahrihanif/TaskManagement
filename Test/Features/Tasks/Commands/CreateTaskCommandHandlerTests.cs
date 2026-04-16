using API.Contracts;
using API.Entities;
using API.Features.Task.Commands.CreateTask;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Test.Features.Tasks.Commands;

public class CreateTaskCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ITaskRepository> _taskRepoMock;
    private readonly Mock<ILogger<CreateTaskCommandHandler>> _loggerMock;
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandHandlerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _taskRepoMock = new Mock<ITaskRepository>();
        _loggerMock = new Mock<ILogger<CreateTaskCommandHandler>>();

        _uowMock.Setup(u => u.Tasks).Returns(_taskRepoMock.Object);

        _handler = new CreateTaskCommandHandler(
            _uowMock.Object,
            _loggerMock.Object);
    }

    // Scenario 1: Valid command returns correct DTO
    [Fact]
    public async Task Handle_WithValidCommand_CreatesTaskAndReturnsDto()
    {
        // Arrange
        var command = new CreateTaskCommand(
            Title: "New Task",
            Description: "Test description",
            Priority: "High",
            DueDate: DateTime.UtcNow.AddDays(7),
            AssigneeEmail: null,
            CategoryId: 1
        );

        _taskRepoMock
           .Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
           .ReturnsAsync((TaskItem t) =>
            {
                t.Id = 42;
                return t;
            });

        _taskRepoMock
           .Setup(r => r.GetByIdAsync(42))
           .ReturnsAsync(new TaskItem
            {
                Id = 42,
                Title = "New Task",
                Priority = TaskPriority.High,
                Status = TaskItemStatus.Todo,
                Category = new Category
                {
                    Name = "Development"
                }
            });

        _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(42);
        result.Title.Should().Be("New Task");
        result.Priority.Should().Be("High");
        result.CategoryName.Should().Be("Development");
    }

    // Scenario 2: AddAsync and SaveChangesAsync called correctly
    [Fact]
    public async Task Handle_WithValidCommand_CallsAddAsyncAndSaveChanges()
    {
        // Arrange
        var command = new CreateTaskCommand("Task", null, "Low", null, null, 1);

        _taskRepoMock
           .Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
           .ReturnsAsync((TaskItem t) => t);

        _taskRepoMock
           .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
           .ReturnsAsync(new TaskItem
            {
                Title = "Task",
                Priority = TaskPriority.Low,
                Status = TaskItemStatus.Todo,
                Category = new Category
                {
                    Name = "Dev"
                }
            });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _taskRepoMock.Verify(
            r => r.AddAsync(It.Is<TaskItem>(t => t.Title == "Task")),
            Times.Once);

        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    // Scenario 3: Entity properties mapped correctly from command
    [Fact]
    public async Task Handle_WithValidCommand_SetsCorrectPriorityOnEntity()
    {
        // Arrange
        var command = new CreateTaskCommand("Task", null, "Medium", null, null, 1);
        TaskItem? capturedTask = null;

        _taskRepoMock
           .Setup(r => r.AddAsync(It.IsAny<TaskItem>()))
           .Callback<TaskItem>(t => capturedTask = t)
           .ReturnsAsync((TaskItem t) => t);

        _taskRepoMock
           .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
           .ReturnsAsync(new TaskItem
            {
                Title = "Task",
                Priority = TaskPriority.Medium,
                Status = TaskItemStatus.Todo,
                Category = new Category
                {
                    Name = "Dev"
                }
            });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        capturedTask.Should().NotBeNull();
        capturedTask!.Priority.Should().Be(TaskPriority.Medium);
        capturedTask.Status.Should().Be(TaskItemStatus.Todo);
    }
}