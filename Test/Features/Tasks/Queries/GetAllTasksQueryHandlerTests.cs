using API.Contracts;
using API.Entities;
using API.Features.Task.Queries.GetAllTasks;
using FluentAssertions;
using Moq;

namespace Test.Features.Tasks.Queries;

public class GetAllTasksQueryHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly GetAllTasksQueryHandler _handler;

    public GetAllTasksQueryHandlerTests()
    {
        _uowMock = new Mock<IUnitOfWork>();
        _handler = new GetAllTasksQueryHandler(_uowMock.Object);
    }

    // Scenario 1: Repository returns 2 tasks with categories
    [Fact]
    public async Task Handle_WhenTasksExist_ReturnsAllTasksAsDtos()
    {
        // Arrange
        var tasks = new List<TaskItem>
        {
            new() { Id = 1, Title = "Task A", Status = TaskItemStatus.Todo,
                Priority = TaskPriority.High,
                Category = new Category { Name = "Development" } },
            new() { Id = 2, Title = "Task B", Status = TaskItemStatus.Done,
                Priority = TaskPriority.Low,
                Category = new Category { Name = "Testing" } }
        };

        var mockTaskRepo = new Mock<ITaskRepository>();
        mockTaskRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);
        _uowMock.Setup(u => u.Tasks).Returns(mockTaskRepo.Object);

        // Act
        var result = await _handler.Handle(
            new GetAllTasksQuery(), CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.First().Title.Should().Be("Task A");
        result.First().CategoryName.Should().Be("Development");
        result.Last().Title.Should().Be("Task B");
        result.Last().Status.Should().Be("Done");
    }

    // Scenario 2: Repository returns empty list
    [Fact]
    public async Task Handle_WhenNoTasksExist_ReturnsEmptyCollection()
    {
        // Arrange
        var mockTaskRepo = new Mock<ITaskRepository>();
        mockTaskRepo.Setup(r => r.GetAllAsync())
                    .ReturnsAsync(new List<TaskItem>());
        _uowMock.Setup(u => u.Tasks).Returns(mockTaskRepo.Object);

        // Act
        var result = await _handler.Handle(
            new GetAllTasksQuery(), CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    // Scenario 3: Verify GetAllAsync called exactly once
    [Fact]
    public async Task Handle_WhenTasksExist_CallsGetAllAsyncOnce()
    {
        // Arrange
        var mockTaskRepo = new Mock<ITaskRepository>();
        mockTaskRepo.Setup(r => r.GetAllAsync())
                    .ReturnsAsync(new List<TaskItem>());
        _uowMock.Setup(u => u.Tasks).Returns(mockTaskRepo.Object);

        // Act
        await _handler.Handle(new GetAllTasksQuery(), CancellationToken.None);

        // Assert
        mockTaskRepo.Verify(r => r.GetAllAsync(), Times.Once);
    }
}