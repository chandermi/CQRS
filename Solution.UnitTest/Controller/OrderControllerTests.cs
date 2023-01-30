using Microsoft.AspNetCore.Mvc;
using Moq;
using Solution.Controllers;
using Solution.DAL;
using Solution.Dipatcher;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.CommandHandler.Order.Command;
using Solution.Infrastructure.CommandHandler.Order.CustomerQueryHandler;

namespace Solution.UnitTest
{
    [TestFixture]
    public class OrderControllerTests
    {
        private OrderController _orderController;
        private Mock<ICommandDispatcher> _commandDispatcherMock;
        private Mock<IQueryDispatcher> _queryDispatcherMock;
        [SetUp]
        public void SetUp()
        {
            _commandDispatcherMock = new Mock<ICommandDispatcher>();
            _queryDispatcherMock = new Mock<IQueryDispatcher>();
            _orderController = new OrderController(_commandDispatcherMock.Object, _queryDispatcherMock.Object);
        }

        [Test]
        public void CreateOrder_GivenNullCommand_ThrowsArgumentNullException()
        {
            // Arrange
            CreateOrderCommand command = null;

            // Assert
            Assert.Throws<ArgumentNullException>(() => _orderController.CreateOrder(command));
        }

        [Test]
        public async Task CreateOrder_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new CreateOrderCommand();
            _commandDispatcherMock.Setup(x => x.Dispatch(command));

            // Act
            var result = _orderController.CreateOrder(command);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task CreateOrder_ThrowsException_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var command = new CreateOrderCommand();
            _commandDispatcherMock.Setup(x => x.Dispatch(command)).Throws(new Exception("Test message"));

            // Act
            var result = _orderController.CreateOrder(command);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var objectResult = result as BadRequestObjectResult;
            Assert.AreEqual("Unable to save order. Test message", objectResult.Value);
        }

        [Test]
        public async Task GetOrder_Returns_OkObjectResult_With_List_Of_Orders()
        {
            // Arrange
            var customerId = 1;
            var orders = new List<Order>
            {
                new Order { Id = 1, CustomerId = 1, OrderDate = new DateTime(), TotalPrice = 10, Items = new List<Item>() },
                new Order { Id = 2, CustomerId = 1, OrderDate = new DateTime(), TotalPrice = 20, Items = new List<Item>() }
            };

            _queryDispatcherMock.Setup(x => x.HandleAsync<GetOrderQuery, List<Order>>(It.IsAny<GetOrderQuery>()))
                .ReturnsAsync(orders);

            // Act
            var result = await _orderController.GetOrder(customerId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            var returnedOrders = objectResult.Value as List<Order>;
            Assert.AreEqual(orders, returnedOrders);
        }

        [Test]
        public async Task GetOrder_Throws_ArgumentNullException_When_CustomerId_Is_0()
        {
            // Arrange
            var customerId = 0;

            // Act and Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => _orderController.GetOrder(customerId));
            Assert.AreEqual(nameof(customerId), ex.ParamName);
        }

        [Test]
        public async Task GetOrder_Returns_BadRequestObjectResult_When_Exception_Is_Thrown()
        {
            // Arrange
            var customerId = 1;
            var exceptionMessage = "Unable to process your request.";
            _queryDispatcherMock.Setup(x => x.HandleAsync<GetOrderQuery, List<Order>>(It.IsAny<GetOrderQuery>()))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _orderController.GetOrder(customerId);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }


        [Test]
        public async Task GetOrdersIterateByDate_WhenCalledWithValidCustomerId_ReturnsOkResult()
        {
            // Arrange
            var customerId = 1;
            var orderby = "asc";
            var orders = new List<Solution.DAL.Order>();
            _queryDispatcherMock.Setup(x => x.HandleAsync<CustomerOrderDateCommandQuery, List<Solution.DAL.Order>>(It.IsAny<CustomerOrderDateCommandQuery>()))
                .Returns(Task.FromResult(orders));

            // Act
            var result = await _orderController.GetOrdersIterateByDate(customerId, orderby);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            Assert.AreEqual(orders, ((OkObjectResult)result).Value);
        }

        [Test]
        public async Task GetOrdersIterateByDate_WhenCalledWithInvalidCustomerId_ThrowsArgumentNullException()
        {
            // Arrange
            var customerId = 0;
            var orderby = "asc";

            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _orderController.GetOrdersIterateByDate(customerId, orderby));
        }

        [Test]
        public async Task GetOrdersIterateByDate_WhenQueryDispatcherThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var customerId = 1;
            var orderby = "asc";
            _queryDispatcherMock.Setup(x => x.HandleAsync<CustomerOrderDateCommandQuery, List<Solution.DAL.Order>>(It.IsAny<CustomerOrderDateCommandQuery>()))
                .ThrowsAsync(new Exception("Unable to process your request."));

            // Act
            var result = await _orderController.GetOrdersIterateByDate(customerId, orderby);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}
