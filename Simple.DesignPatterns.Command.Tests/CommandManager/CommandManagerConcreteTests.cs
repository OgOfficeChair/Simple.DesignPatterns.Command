using Microsoft.Extensions.Logging;
using Moq;

namespace Simple.DesignPatterns.Command.Tests.CommandManager;

[TestClass]
public class CommandManagerConcreteTests
{
    private DesignPatterns.Command.CommandManager _commandManager = null!;
    [TestInitialize]
    public void Setup()
    {
        var logger = new Mock<ILogger<DesignPatterns.Command.CommandManager>>().Object;
        _commandManager = new DesignPatterns.Command.CommandManager(logger);
    }

    [TestMethod]
    public async Task ExecuteAsync_ConcreteCommand_ExecutesSuccessfully()
    {
        // Arrange
        var store = new CurrentNumberStore();
        var startNumber = store.SetNumber(0);
        var command = new AddOneCommand(store);

        // Act
        await _commandManager.ExecuteAsync(command);
        // Assert
        Assert.IsTrue(store.GetNumber() == 1);
    }

    [TestMethod]
    public async Task ExecuteAsync_ConcreteCommand_UndosSuccessfully()
    {
        // Arrange
        var store = new CurrentNumberStore();
        var startNumber = store.SetNumber(0);
        var command = new AddOneCommand(store);

        // Act
        await _commandManager.ExecuteAsync(command);
        await _commandManager.UndoAsync();

        // Assert
        Assert.IsTrue(store.GetNumber() == 0);
    }
}

public class AddOneCommand : ICommand
{
    private readonly CurrentNumberStore _store;

    public AddOneCommand(CurrentNumberStore store)
    {
        _store = store;
    }
    public async Task ExecuteAsync()
    {
        var currentNumber = _store.GetNumber();
        _store.SetNumber(currentNumber + 1);
    }

    public async Task UndoAsync()
    {
        var currentNumber = _store.GetNumber();
        _store.SetNumber(currentNumber - 1);
    }
}

public class CurrentNumberStore 
{
    private int CurrentNumber { get; set; } = default;

    public int GetNumber()
    {
        return CurrentNumber;
    }

    public int SetNumber(int value) 
    { 
        CurrentNumber = value;
        return GetNumber();
    }
}