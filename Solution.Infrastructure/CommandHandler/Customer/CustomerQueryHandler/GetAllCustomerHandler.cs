using Solution.Infrastructure.Repository;

namespace Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler
{
    public class GetAllCustomerHandler : IQueryHandler<GetAllCustomerQuery,List<Solution.DAL.Customer>>
    {
        private readonly ICustomerRespository _customerRespository;

        public GetAllCustomerHandler(ICustomerRespository customerRespository)
        {
            _customerRespository = customerRespository;
        }

        public async Task<List<Solution.DAL.Customer>> HandleAsync(GetAllCustomerQuery query)
        {
            return await _customerRespository.GetCustomers(query);
        }
    }
}
