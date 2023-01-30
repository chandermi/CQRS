using Moq;
using Solution.DAL;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.Repository;

namespace Solution.UnitTest.QueryHandler
{
    [TestFixture]
    public class GetCustomerHandlerTests
    {
        private Mock<ICustomerRespository> _customerRepositoryMock;
        private GetCustomerHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _customerRepositoryMock = new Mock<ICustomerRespository>();
            _handler = new GetCustomerHandler(_customerRepositoryMock.Object);
        }

        [Test]
        public async Task HandleAsync_Returns_Customer_When_Found()
        {
            // Arrange
            var customer = new Solution.DAL.Customer("John", "Doe", "", "");
            _customerRepositoryMock.Setup(x => x.GetCustomerById(It.IsAny<GetCustomerQuery>())).ReturnsAsync(customer);
            var query = new GetCustomerQuery { Id = 1 };

            // Act
            var result = await _handler.HandleAsync(query);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.FirstName, Is.EqualTo("John"));
            Assert.That(result.LastName, Is.EqualTo("Doe"));
        }

        [Test]
        public async Task HandleAsync_Returns_Null_When_Customer_Not_Found()
        {
            // Arrange
            _customerRepositoryMock.Setup(x => x.GetCustomerById(It.IsAny<GetCustomerQuery>())).ReturnsAsync((Customer)null);
            var query = new GetCustomerQuery { Id = 1 };

            // Act
            var result = await _handler.HandleAsync(query);

            // Assert
            Assert.IsNull(result);
        }


    }
}
