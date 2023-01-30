using Moq;
using Solution.Infrastructure.CommandHandler.Customer.DeleteCommand;
using Solution.Infrastructure.Repository;

namespace Solution.UnitTest.CommandHandler
{
    [TestFixture]
    public class DeleteCustomerCommandHandlerTests
    {
        private DeleteCustomerCommandHandler _sut;
        private Mock<ICustomerRespository> _mockCustomerRepository;


        [SetUp]
        public void Setup()
        {
            _mockCustomerRepository = new Mock<ICustomerRespository>();
            _sut = new DeleteCustomerCommandHandler(_mockCustomerRepository.Object);
        }

        [Test]
        public async Task Handle_ValidCommand_ShouldDeleteCustomer()
        {
            //Arrange
            var command = new DeleteCustomerCommand { Id = 1 };
            _mockCustomerRepository.Setup(x => x.DeleteCustomer(command)).ReturnsAsync(true);

            //Act
            _sut.Handle(command);

            //Assert
            _mockCustomerRepository.Verify(x => x.DeleteCustomer(command), Times.Once);
        }

        [Test]
        public async Task Handle_InvalidCommand_ShouldNotDeleteCustomer()
        {
            //Arrange
            var command = new DeleteCustomerCommand { Id = 0 };
            _mockCustomerRepository.Setup(x => x.DeleteCustomer(command)).ReturnsAsync(false);

            //Act
            _sut.Handle(command);

            //Assert
            _mockCustomerRepository.Verify(x => x.DeleteCustomer(command), Times.Once);
        }
    }
}
