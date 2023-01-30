using Microsoft.Extensions.DependencyInjection;
using Solution.Infrastructure.CommandHandler;
using Solution.Infrastructure.CommandHandler.Customer;
using Solution.Infrastructure.CommandHandler.Customer.CreateCommand;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.CommandHandler.Customer.DeleteCommand;
using Solution.Infrastructure.CommandHandler.Customer.UpdateCommand;
using Solution.Infrastructure.CommandHandler.Order.Command;
using Solution.Infrastructure.CommandHandler.Order.CreateCommand;
using Solution.Infrastructure.CommandHandler.Order.CustomerQueryHandler;
using Solution.Infrastructure.Repository;
using Solution.Infrastructure.Service;
using Solution.Infrastructure.UOW;
namespace Solution.Infrastructure
{
    public class DependecyInjection
    {
        public DependecyInjection()
        { 
        
        }

        public void RegisterComponents(IServiceCollection Services)
        {
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<ICommandHandler<CreateOrderCommand>, OrderCommandHandler>();
            Services.AddScoped<ICommandHandler<CreateCustomerCommand>, CreateCustomerCommandHandler>();
            Services.AddScoped<ICommandHandler<UpdateCustomerCommand>, UpdateCustomerCommandHandler>();
            Services.AddScoped<ICommandHandler<DeleteCustomerCommand>, DeleteCustomerCommandHandler>();
            Services.AddScoped<IQueryHandler<GetOrderQuery, List<Solution.DAL.Order>>, GetOrderHandler>();
            Services.AddScoped<IQueryHandler<GetCustomerQuery, Solution.DAL.Customer>, GetCustomerHandler>();
            Services.AddScoped<IQueryHandler<GetAllCustomerQuery, List<Solution.DAL.Customer>>, GetAllCustomerHandler>();
            Services.AddScoped<IQueryHandler<CustomerOrderDateCommandQuery, List<Solution.DAL.Order>>, CustomerOrderDateCommandHandler>();
            Services.AddScoped<ICustomerRespository, CustomerService>();
            Services.AddScoped<IOrderRespository, OrderService>();
        }
    }
}
