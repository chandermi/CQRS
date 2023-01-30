
using Solution.Infrastructure.Repository;

namespace Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler
{
    public class GetCustomerHandler : IQueryHandler<GetCustomerQuery,Solution.DAL.Customer>
    {
        private readonly ICustomerRespository _customerRespository;

        public GetCustomerHandler(ICustomerRespository customerRespository)
        {
            _customerRespository = customerRespository;
        }

        public async Task<Solution.DAL.Customer> HandleAsync(GetCustomerQuery query)
        {
            return await _customerRespository.GetCustomerById(query);
        }
    }
}
