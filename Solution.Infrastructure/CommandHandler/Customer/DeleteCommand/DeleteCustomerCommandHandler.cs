using Solution.Infrastructure.Repository;

namespace Solution.Infrastructure.CommandHandler.Customer.DeleteCommand
{
    public class DeleteCustomerCommandHandler : ICommandHandler<DeleteCustomerCommand>
    {
        private readonly ICustomerRespository _customerRespository;

        public DeleteCustomerCommandHandler(ICustomerRespository customerRespository)
        {
            _customerRespository = customerRespository;
        }

        public void Handle(DeleteCustomerCommand command)
        {
            _customerRespository.DeleteCustomer(command);
        }
    }

}
