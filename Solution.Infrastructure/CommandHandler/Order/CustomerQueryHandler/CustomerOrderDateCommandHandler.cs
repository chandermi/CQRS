using Solution.Infrastructure.Repository;

namespace Solution.Infrastructure.CommandHandler.Order.CustomerQueryHandler
{
    public class CustomerOrderDateCommandHandler : IQueryHandler<CustomerOrderDateCommandQuery, List<Solution.DAL.Order>>
    {
        private readonly IOrderRespository _orderRespository;


        public CustomerOrderDateCommandHandler(IOrderRespository orderRespository)
        {
            _orderRespository = orderRespository;
        }
        public async Task<List<Solution.DAL.Order>> HandleAsync(CustomerOrderDateCommandQuery query)
        {
            return await _orderRespository.GetOrderByDate(query);
        }
    }
}
