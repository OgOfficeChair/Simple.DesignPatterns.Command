using Microsoft.Extensions.Logging;

namespace Simple.DesignPatterns.Command;

public class CommandManager : ICommandManager<ICommand>
{
    private readonly ILogger<CommandManager> _logger;
    private readonly Stack<ICommand> _commandStack = new Stack<ICommand>();

    public CommandManager(ILogger<CommandManager> logger)
    {
        _logger = logger;
    }

    public async Task ExecuteAsync(ICommand command)
    {
        if (command == null)
            throw new ArgumentNullException(nameof(command));

        _commandStack.Push(command);
        await command.ExecuteAsync();
    }

    public async Task UndoAsync()
    {
        for (var i = 0; i < _commandStack.Count; i++)
        {
            var command = _commandStack.Pop();
            try
            {
                _logger.LogDebug("Undoing command of type {CommandType}", command.GetType());
                await command.UndoAsync();
                _logger.LogInformation("Command of type {CommandType} was undone successfully", command.GetType());
            }
            catch (NotImplementedException ex)
            {
                _logger.LogInformation(ex, "Undo operation not implemented for command of type {CommandType}", command.GetType());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while undoing command of type {CommandType}", command.GetType());
            }
        }
    }
}

public interface ICommandManager<TCommand>
{
    Task ExecuteAsync(TCommand command);
    Task UndoAsync();
}
