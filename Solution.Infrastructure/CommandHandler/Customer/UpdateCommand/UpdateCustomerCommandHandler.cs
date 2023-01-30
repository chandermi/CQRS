using Solution.Infrastructure.CommandHandler.Customer.UpdateCommand;
using Solution.Infrastructure.Repository;

namespace Solution.Infrastructure.CommandHandler.Customer
{
    public class UpdateCustomerCommandHandler : ICommandHandler<UpdateCustomerCommand>
    {
        private readonly ICustomerRespository _customerRespository;

        public UpdateCustomerCommandHandler(ICustomerRespository customerRespository)
        {
            _customerRespository = customerRespository;
        }

        public void Handle(UpdateCustomerCommand command)
        {
            _customerRespository.UpdateCustomer(command);
        }
    }
}
