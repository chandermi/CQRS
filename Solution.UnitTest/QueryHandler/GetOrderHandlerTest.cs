using Moq;
using Solution.DAL;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.Repository;

namespace Solution.UnitTest.QueryHandler
{
    [TestFixture]
    public class GetOrderHandlerTests
    {
        private GetOrderHandler _getOrderHandler;
        private Mock<IOrderRespository> _orderRepositoryMock;
        private GetOrderQuery _query;


        [SetUp]
        public void SetUp()
        {
            _orderRepositoryMock = new Mock<IOrderRespository>();
            _getOrderHandler = new GetOrderHandler(_orderRepositoryMock.Object);
            _query = new GetOrderQuery { customerId = 1 };
        }

        [Test]
        public async Task HandleAsync_ShouldCallGetOrdersWithCorrectArgument()
        {
            // Arrange
            var expectedOrders = new List<Order>();
            _orderRepositoryMock.Setup(x => x.GetOrders(_query)).ReturnsAsync(expectedOrders);

            // Act
            await _getOrderHandler.HandleAsync(_query);

            // Assert
            _orderRepositoryMock.Verify(x => x.GetOrders(_query), Times.Once);
        }

        [Test]
        public async Task HandleAsync_ShouldReturnExpectedOrders()
        {
            // Arrange
            var expectedOrders = new List<Order>();
            _orderRepositoryMock.Setup(x => x.GetOrders(_query)).ReturnsAsync(expectedOrders);

            // Act
            var result = await _getOrderHandler.HandleAsync(_query);

            // Assert
            Assert.AreEqual(expectedOrders, result);
        }
    }
}
