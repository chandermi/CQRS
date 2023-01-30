using NSubstitute;
using Solution.DAL;
using Solution.Infrastructure.CommandHandler.Order.CustomerQueryHandler;
using Solution.Infrastructure.Repository;

namespace Solution.UnitTest.QueryHandler
{
    [TestFixture]
    public class CustomerOrderDateCommandHandlerTests
    {
        private IOrderRespository _orderRepository;
        private CustomerOrderDateCommandHandler _commandHandler;

        [SetUp]
        public void SetUp()
        {
            _orderRepository = Substitute.For<IOrderRespository>();
            _commandHandler = new CustomerOrderDateCommandHandler(_orderRepository);
        }

        [Test]
        public async Task HandleAsync_ShouldReturnOrdersForGivenCustomerId()
        {
            // Arrange
            var customerId = 1;
            var order1 = new Order { Id = 1, CustomerId = customerId, OrderDate = new DateTime(2021, 1, 1) };
            var order2 = new Order { Id = 2, CustomerId = customerId, OrderDate = new DateTime(2021, 2, 1) };
            var order3 = new Order { Id = 3, CustomerId = customerId, OrderDate = new DateTime(2021, 3, 1) };
            var orders = new List<Order> { order1, order2, order3 };
            _orderRepository.GetOrderByDate(Arg.Any<CustomerOrderDateCommandQuery>()).Returns(orders);

            // Act
            var query = new CustomerOrderDateCommandQuery { customerId = customerId };
            var result = await _commandHandler.HandleAsync(query);

            // Assert
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.IsTrue(result.All(o => o.CustomerId == customerId));
            _ = _orderRepository.Received().GetOrderByDate(Arg.Any<CustomerOrderDateCommandQuery>());
        }
    }
}
