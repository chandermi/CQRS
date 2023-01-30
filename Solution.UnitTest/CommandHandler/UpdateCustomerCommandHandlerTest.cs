using NSubstitute;
using Solution.Infrastructure.CommandHandler.Customer;
using Solution.Infrastructure.CommandHandler.Customer.UpdateCommand;
using Solution.Infrastructure.Repository;

namespace Solution.UnitTest.CommandHandler
{
    [TestFixture]
    public class UpdateCustomerCommandHandlerTests
    {
        private ICustomerRespository _customerRespository;
        private UpdateCustomerCommandHandler _updateCustomerCommandHandler;

        [SetUp]
        public void SetUp()
        {
            // Create a mock of the ICustomerRespository interface
            _customerRespository = Substitute.For<ICustomerRespository>();
            _updateCustomerCommandHandler = new UpdateCustomerCommandHandler(_customerRespository);
        }

        [Test]
        public void Handle_CallsUpdateCustomerMethodOnCustomerRepository()
        {
            // Arrange
            var updateCustomerCommand = new UpdateCustomerCommand
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                PostalCode = "12345"
            };

            // Act
            _updateCustomerCommandHandler.Handle(updateCustomerCommand);

            // Assert
            _customerRespository.Received().UpdateCustomer(updateCustomerCommand);
        }
    }

}
