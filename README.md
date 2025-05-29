# Simple.DesignPatterns.Command

`Simple.DesignPatterns.Command` is a C# library that implements the boiler plate code for the Command design pattern, so you don't have to recreate it in every project.

This package assumes that your commands are instantiated with data required to execute the command.

If you're unfamiliar with this pattern, you can learn more about it on [Wikipedia](https://en.wikipedia.org/wiki/Command_pattern).

## Features

- **Command Management**: Execute and undo commands using the `CommandManager`.
- **Logging**: Integrated logging using `Microsoft.Extensions.Logging` to track command execution and undo operations.
- **Asynchronous Operations**: Supports asynchronous command execution and undoing.

## Requirements
This package assumes 
- Use of [dependency injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- Use of [logging](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line)

## Installation

You can install the package via NuGet Package Manager:

```bash
Install-Package Simple.DesignPatterns.Command
```

Or using the .NET CLI:
```bash
dotnet add package Simple.DesignPatterns.Command
```

## Usage:
#### Command Interface
To create a command, implement the ICommand interface:
```cs
public class MyCommand : Command
{
    public override async Task ExecuteAsync()
    {
        // Implementation of command execution
    }

    public override async Task UndoAsync()
    {
        // Implementation of command undo
    }
}
```

#### Command Manager
Use the CommandManager to execute and undo commands:
```cs
using Microsoft.Extensions.Logging;
using Simple.DesignPatterns.Command;

public class Example
{
    private readonly ICommandManager<ICommand> _commandManager;

    public Example(ILogger<CommandManager> logger)
    {
        _commandManager = new CommandManager(logger);
    }

    public async Task Run()
    {
        try 
        {
            var command = new MyCommand();
            await _commandManager.ExecuteAsync(command);
        }
        catch (Exception ex) 
        {
            // To undo the command
            await _commandManager.UndoAsync();
        }
    }
}
```

#### Example
```cs
var store = new Store();
var command = new MyCommand(store);
var commandManager = new CommandManager(Logger.Log);
await _commandManager.ExecuteAsync(command);
```

#### Logging
The CommandManager logs the execution and undoing of commands. Ensure that you have configured logging in your application to see the logs.

### Exception Handling
The CommandManager handles exceptions during command execution and undoing. If a command's undo operation is not implemented, a NotImplementedException will be logged. Other exceptions will be logged as errors.

### Contributing
Contributions are welcome! Please feel free to submit a pull request or open an issue for any enhancements or bug fixes.

### License
This project is licensed under the MIT License. See the [LICENSE](./LICENSE.txt) file for details.