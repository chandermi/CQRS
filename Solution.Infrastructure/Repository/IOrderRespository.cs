using Solution.DAL;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.CommandHandler.Order.Command;
using Solution.Infrastructure.CommandHandler.Order.CustomerQueryHandler;

namespace Solution.Infrastructure.Repository
{
    public interface IOrderRespository
    {
        Task<int> CreateOrder(CreateOrderCommand command);
        
        Task<List<Order>> GetOrderByDate(CustomerOrderDateCommandQuery query);

        Task<List<Solution.DAL.Order>> GetOrders(GetOrderQuery query);
    }
}
