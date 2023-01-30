namespace Solution.Infrastructure.CommandHandler
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand command);
    }
}
