using Solution.Infrastructure.CommandHandler.Order.Command;
using Solution.Infrastructure.Repository;

namespace Solution.Infrastructure.CommandHandler.Order.CreateCommand
{
    public class OrderCommandHandler : ICommandHandler<CreateOrderCommand>
    {
        private readonly IOrderRespository _orderRespository;


        public OrderCommandHandler(IOrderRespository orderRespository)
        {
            _orderRespository = orderRespository;
        }

        public void Handle(CreateOrderCommand command)
        {
            _orderRespository.CreateOrder(command);
        }
    }
}
