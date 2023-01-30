using log4net;
using Microsoft.EntityFrameworkCore;
using Solution.DAL;
using Solution.Helper;
using Solution.Infrastructure.CommandHandler.Customer.CreateCommand;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.CommandHandler.Customer.DeleteCommand;
using Solution.Infrastructure.CommandHandler.Customer.UpdateCommand;
using Solution.Infrastructure.Repository;
using Solution.Infrastructure.UOW;
using System.Reflection;

namespace Solution.Infrastructure.Service
{
    public class CustomerService : ICustomerRespository
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateCustomer(CreateCustomerCommand cmd)
        {
             var customer = new Customer(
              cmd.FirstName,
              cmd.LastName,
              cmd.Address,
              cmd.PostalCode
            );

            await _unitOfWork.Customers.AddAsync(customer);
            _unitOfWork.SaveChanges();
            return customer.Id;
        }

        public async Task<bool> DeleteCustomer(DeleteCustomerCommand cmd)
        {
            try
            {
                var orders = await _unitOfWork.Orders.Where(a => a.CustomerId == cmd.Id).ToListAsync();
                _unitOfWork.Orders.RemoveRange(orders);
                var customer = await _unitOfWork.Customers.FindAsync(cmd.Id);
                if (customer == null)
                {
                    throw new NotFoundException($"customer doesn't exists");
                }
                _unitOfWork.Customers.Remove(customer);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Failed to delete customer", ex.Message);
                return false;
            }
            
        }

        public async Task<Customer> GetCustomerById(GetCustomerQuery query)
        {
            var customer = await _unitOfWork.Customers.FindAsync(query.Id);
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            return customer;
        }

        public async Task<List<Customer>> GetCustomers(GetAllCustomerQuery query)
        {
            var customer = await _unitOfWork.Customers.ToListAsync();
            if (customer == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            return customer;
        }

        public async Task<bool> UpdateCustomer(UpdateCustomerCommand cmd)
        {
            try
            {
                var customer = await _unitOfWork.Customers.FindAsync(cmd.Id);
                if (customer == null)
                {
                    throw new ArgumentNullException(nameof(cmd));
                }

                customer.FirstName = cmd.FirstName;
                customer.LastName = cmd.LastName;
                customer.Address = cmd.Address;
                customer.PostalCode = cmd.PostalCode;
                _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Failed to update customer", ex.Message);
                return false;
            }
            
        }
    }
}
