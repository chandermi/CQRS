using Solution.Infrastructure.Repository;

namespace Solution.Infrastructure.CommandHandler.Customer.CreateCommand
{
    public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand>
    {
        private readonly ICustomerRespository _customerRespository;


        public CreateCustomerCommandHandler(ICustomerRespository customerRespository)
        {
            _customerRespository = customerRespository;
        }

        public void Handle(CreateCustomerCommand command)
        {
            _customerRespository.CreateCustomer(command);
        }
    }
}
