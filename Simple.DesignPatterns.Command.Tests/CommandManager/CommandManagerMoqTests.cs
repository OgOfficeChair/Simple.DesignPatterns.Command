using Microsoft.Extensions.Logging;
using Moq;

namespace Simple.DesignPatterns.Command.Tests.CommandManager;

[TestClass]
public sealed class CommandManagerMoqTests
{
    private Mock<ILogger<DesignPatterns.Command.CommandManager>> _loggerMock = null!;
    private DesignPatterns.Command.CommandManager _commandManager = null!;

    [TestInitialize]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<DesignPatterns.Command.CommandManager>>();
        _commandManager = new DesignPatterns.Command.CommandManager(_loggerMock.Object);
    }

    [TestMethod]
    public async Task ExecuteAsync_PushesCommandAndExecutes()
    {
        // Arrange
        var commandMock = new Mock<ICommand>();
        commandMock.Setup(c => c.ExecuteAsync()).Returns(Task.CompletedTask).Verifiable();

        // Act
        await _commandManager.ExecuteAsync(commandMock.Object);

        // Assert
        commandMock.Verify(c => c.ExecuteAsync(), Times.Once);
    }

    [TestMethod]
    public async Task ExecuteAsync_NullCommand_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
        {
            await _commandManager.ExecuteAsync(null!);
        });
    }

    [TestMethod]
    public async Task UndoAsync_PopsAndUndoesAllCommands()
    {
        // Arrange
        var commandMock1 = new Mock<ICommand>();
        var commandMock2 = new Mock<ICommand>();
        commandMock1.Setup(c => c.ExecuteAsync()).Returns(Task.CompletedTask);
        commandMock2.Setup(c => c.ExecuteAsync()).Returns(Task.CompletedTask);
        commandMock1.Setup(c => c.UndoAsync()).Returns(Task.CompletedTask).Verifiable();
        commandMock2.Setup(c => c.UndoAsync()).Returns(Task.CompletedTask).Verifiable();

        await _commandManager.ExecuteAsync(commandMock1.Object);
        await _commandManager.ExecuteAsync(commandMock2.Object);

        // Act
        await _commandManager.UndoAsync();

        // Assert
        commandMock1.Verify(c => c.UndoAsync(), Times.Once);
        commandMock2.Verify(c => c.UndoAsync(), Times.Once);
    }

    [TestMethod]
    public async Task UndoAsync_UndoNotImplemented_LogsInformation()
    {
        // Arrange
        var commandMock = new Mock<ICommand>();
        commandMock.Setup(c => c.ExecuteAsync()).Returns(Task.CompletedTask);
        commandMock.Setup(c => c.UndoAsync()).ThrowsAsync(new NotImplementedException());

        await _commandManager.ExecuteAsync(commandMock.Object);

        // Act
        await _commandManager.UndoAsync();

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Undo operation not implemented")),
                It.IsAny<NotImplementedException>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once
        );
    }

    [TestMethod]
    public async Task UndoAsync_UnexpectedException_LogsError()
    {
        // Arrange
        var commandMock = new Mock<ICommand>();
        commandMock.Setup(c => c.ExecuteAsync()).Returns(Task.CompletedTask);
        commandMock.Setup(c => c.UndoAsync()).ThrowsAsync(new InvalidOperationException("Test error"));

        await _commandManager.ExecuteAsync(commandMock.Object);

        // Act
        await _commandManager.UndoAsync();

        // Assert
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("An error occurred while undoing command")),
                It.IsAny<InvalidOperationException>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once
        );
    }
}
