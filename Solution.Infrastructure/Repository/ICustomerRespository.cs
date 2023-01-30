using Solution.Infrastructure.CommandHandler.Customer.CreateCommand;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.CommandHandler.Customer.DeleteCommand;
using Solution.Infrastructure.CommandHandler.Customer.UpdateCommand;

namespace Solution.Infrastructure.Repository
{
    public interface ICustomerRespository
    {
        Task<int> CreateCustomer(CreateCustomerCommand cmd);
        Task<List<Solution.DAL.Customer>> GetCustomers(GetAllCustomerQuery query);
        Task<Solution.DAL.Customer> GetCustomerById(GetCustomerQuery query);
        Task<bool> UpdateCustomer(UpdateCustomerCommand cmd);
        Task<bool> DeleteCustomer(DeleteCustomerCommand cmd);
    }
}
