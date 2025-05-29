namespace Simple.DesignPatterns.Command
{
    public abstract class Command : ICommand
    {
        public abstract Task ExecuteAsync();
        public abstract Task UndoAsync();
    }

    public interface ICommand
    {
        public abstract Task ExecuteAsync();
        public abstract Task UndoAsync();
    }
}
