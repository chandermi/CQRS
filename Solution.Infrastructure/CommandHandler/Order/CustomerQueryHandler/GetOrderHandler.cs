using Solution.Infrastructure.Repository;

namespace Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler
{
    public class GetOrderHandler : IQueryHandler<GetOrderQuery, List<Solution.DAL.Order>>
    {
        private readonly IOrderRespository _orderRespository;


        public GetOrderHandler(IOrderRespository orderRespository)
        {
            _orderRespository = orderRespository;
        }

        public async Task<List<Solution.DAL.Order>> HandleAsync(GetOrderQuery query)
        {
            return await _orderRespository.GetOrders(query);
        }
    }
}
