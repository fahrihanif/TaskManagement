using API.Contracts;
using API.Entities;
using API.Features.Task.Commands.DeleteTask;
using FluentAssertions;
using Moq;

namespace Test.Features.Tasks.Commands;

public class DeleteTaskCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _uowMock;
    private readonly Mock<ITaskRepository> _taskRepoMock;
    private readonly DeleteTaskCommandHandler _handler;

    public DeleteTaskCommandHandlerTests()
    {
        _uowMock      = new Mock<IUnitOfWork>();
        _taskRepoMock = new Mock<ITaskRepository>();
        _uowMock.Setup(u => u.Tasks).Returns(_taskRepoMock.Object);
        _handler = new DeleteTaskCommandHandler(_uowMock.Object);
    }

    // Scenario 1: Task exists — delete succeeds
    [Fact]
    public async Task Handle_WhenTaskExists_DeletesTaskAndReturnsTrue()
    {
        // Arrange
        var task = new TaskItem { Id = 1, Title = "Task to delete" };

        _taskRepoMock
           .Setup(r => r.GetByIdAsync(1))
           .ReturnsAsync(task);

        _uowMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(
            new DeleteTaskCommand(1), CancellationToken.None);

        // Assert
        result.Should().BeTrue();

        _taskRepoMock.Verify(
            r => r.DeleteAsync(It.Is<TaskItem>(t => t.Id == 1)),
            Times.Once);

        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    // Scenario 2: Task does not exist — nothing deleted
    [Fact]
    public async Task Handle_WhenTaskDoesNotExist_ReturnsFalse()
    {
        // Arrange
        _taskRepoMock
           .Setup(r => r.GetByIdAsync(99))
           .ReturnsAsync((TaskItem?)null);

        // Act
        var result = await _handler.Handle(
            new DeleteTaskCommand(99), CancellationToken.None);

        // Assert
        result.Should().BeFalse();

        _taskRepoMock.Verify(
            r => r.DeleteAsync(It.IsAny<TaskItem>()),
            Times.Never);

        _uowMock.Verify(
            u => u.SaveChangesAsync(),
            Times.Never);
    }
}
