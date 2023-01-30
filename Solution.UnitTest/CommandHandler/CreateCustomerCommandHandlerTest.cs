using Moq;
using Solution.Infrastructure.CommandHandler.Customer.CreateCommand;
using Solution.Infrastructure.Repository;

namespace Solution.UnitTest.CommandHandler
{
    [TestFixture]
    public class CreateCustomerCommandHandlerTests
    {
        private CreateCustomerCommandHandler _commandHandler;
        private Mock<ICustomerRespository> _customerRepositoryMock;
        private CreateCustomerCommand _command;
        [SetUp]
        public void SetUp()
        {
            _customerRepositoryMock = new Mock<ICustomerRespository>();
            _commandHandler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object);
            _command = new CreateCustomerCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                PostalCode = "12345"
            };
        }

        [Test]
        public void Handle_ShouldCallCreateCustomerMethod()
        {
            _commandHandler.Handle(_command);
            _customerRepositoryMock.Verify(x => x.CreateCustomer(_command), Times.Once);
        }
    }
}

