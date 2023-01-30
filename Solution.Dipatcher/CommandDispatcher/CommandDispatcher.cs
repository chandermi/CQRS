using Solution.Helper;
using Solution.Infrastructure;
using Solution.Infrastructure.CommandHandler;

namespace Solution.Dipatcher.CommandDispatcher
{

    public class CommandDispatcher : ICommandDispatcher
    {

        private readonly IServiceProvider _serviceProvider;
        
        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }

        public void Dispatch<T>(T command) where T : ICommandDisp
        {
            var service = _serviceProvider.GetService(typeof(ICommandHandler<T>)) as ICommandHandler<T>;
            if (service != null)
                service.Handle(command);
            else
                throw new NotFoundException($"Command doesn't have any handler {command.GetType().Name}");
           
        }
    }
}
