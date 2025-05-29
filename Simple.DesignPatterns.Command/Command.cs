namespace Simple.DesignPatterns.Command;


public interface ICommand
{
    public abstract Task ExecuteAsync();
    public abstract Task UndoAsync();
}
