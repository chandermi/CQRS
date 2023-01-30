using Microsoft.AspNetCore.Mvc;
using Moq;
using Solution.Controllers;
using Solution.DAL;
using Solution.Dipatcher;
using Solution.Infrastructure.CommandHandler.Customer.CreateCommand;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.CommandHandler.Customer.DeleteCommand;
using Solution.Infrastructure.CommandHandler.Customer.UpdateCommand;

namespace Solution.UnitTest
{
    [TestFixture]
    public class CustomerControllerTests
    {
        private CustomerController _controller;
        private Mock<ICommandDispatcher> _commandDispatcherMock;
        private Mock<IQueryDispatcher> _queryDispatcherMock;
        [SetUp]
        public void SetUp()
        {
            _commandDispatcherMock = new Mock<ICommandDispatcher>();
            _queryDispatcherMock = new Mock<IQueryDispatcher>();
            _controller = new CustomerController(_commandDispatcherMock.Object, _queryDispatcherMock.Object);
        }

        [Test]
        public void CreateCustomer_GivenNullCommand_ShouldReturnBadRequest()
        {
            // Act
            var result = _controller.CreateCustomer(null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.That((result as BadRequestObjectResult).Value, Is.EqualTo("Invalid request"));
        }

        [Test]
        public void CreateCustomer_GivenCommand_ShouldCallCommandDispatcher()
        {
            // Arrange
            var command = new CreateCustomerCommand();

            // Act
            _controller.CreateCustomer(command);

            // Assert
            _commandDispatcherMock.Verify(x => x.Dispatch(command), Times.Once);
        }

        [Test]
        public void CreateCustomer_GivenExceptionThrownByDispatcher_ShouldReturnBadRequest()
        {
            // Arrange
            var command = new CreateCustomerCommand();
            _commandDispatcherMock.Setup(x => x.Dispatch(It.IsAny<CreateCustomerCommand>())).Throws(new Exception("Error message"));

            // Act
            var result = _controller.CreateCustomer(command);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.That((result as BadRequestObjectResult).Value, Is.EqualTo("Unable to save customer. Error message"));
        }

        [Test]
        public void CreateCustomer_GivenValidCommand_ShouldReturnOk()
        {
            // Arrange
            var command = new CreateCustomerCommand();

            // Act
            var result = _controller.CreateCustomer(command);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public async Task GetCustomer_ReturnsOkObjectResult_WhenCustomerIsFound()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer("John", "Doe", "123 Main St", "12345");
            customer.Id = customerId;
            _queryDispatcherMock
                .Setup(x => x.HandleAsync<GetCustomerQuery, Customer>(It.IsAny<GetCustomerQuery>()))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomer(customerId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.SameAs(customer));
        }

        [Test]
        public async Task GetCustomer_Throws_ArgumentNullException_When_CustomerId_Is_0()
        {
            // Arrange
            var customerId = 0;

            // Act and Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => _controller.GetCustomer(customerId));
            Assert.AreEqual(nameof(customerId), ex.ParamName);
        }

        [Test]
        public async Task GetAllCustomers_ReturnsOkObjectResult()
        {
            // Arrange
            var customers = new List<Customer> { new Customer("John", "Doe", "123 Main St", "12345") };
            _queryDispatcherMock.Setup(x => x.HandleAsync<GetAllCustomerQuery, List<Customer>>(It.IsAny<GetAllCustomerQuery>()))
                .ReturnsAsync(customers);

            // Act

            var result = await _controller.GetAllCustomers();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var objectResult = result as OkObjectResult;
            Assert.AreEqual(customers, objectResult.Value);
        }

        [Test]
        public async Task GetAllCustomers_ThrowsNullReferenceException_ReturnsBadRequestObjectResult()
        {
            // Arrange
            _queryDispatcherMock.Setup(x => x.HandleAsync<GetAllCustomerQuery, List<Customer>>(It.IsAny<GetAllCustomerQuery>()))
                .ThrowsAsync(new NullReferenceException("Test message"));

            // Act
            var result = await _controller.GetAllCustomers();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var objectResult = result as BadRequestObjectResult;
            Assert.AreEqual("Unable to process your request. Test message", objectResult.Value);
        }

        [Test]
        public void CreateCustomer_ValidCommand_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new CreateCustomerCommand { FirstName = "John", LastName = "Doe", Address = "123 Main St", PostalCode = "12345" };
            _commandDispatcherMock.Setup(x => x.Dispatch<CreateCustomerCommand>(command));


            // Act
            var result = _controller.CreateCustomer(command);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }


        [Test]
        public void UpdateCustomer_ValidCommand_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new UpdateCustomerCommand { Id = 1, FirstName = "John", LastName = "Doe", Address = "123 Main St", PostalCode = "12345" };
            _commandDispatcherMock.Setup(x => x.Dispatch<UpdateCustomerCommand>(command));

            // Act
            var result = _controller.UpdateCustomer(command);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }


        [Test]
        public void DeleteCustomer_ValidCommand_ReturnsOkObjectResult()
        {
            // Arrange
            var command = new DeleteCustomerCommand { Id = 1 };
            _commandDispatcherMock.Setup(x => x.Dispatch<DeleteCustomerCommand>(command));

            // Act
            var result = _controller.DeleteCustomer(command);

            // Assert
            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void DeleteCustomer_InValidCommand_ThrowExceptionArgumentNullException()
        {
            // Arrange
            DeleteCustomerCommand command = null;

            // Act
            Assert.Throws<ArgumentNullException>(() => _controller.DeleteCustomer(command));
        }

        [Test]
        public void DeleteCustomer_InValidCommand_ThrowsException_ReturnsBadRequestObjectResult()
        {
            // Arrange
            var command = new DeleteCustomerCommand { Id = 1 };
            _commandDispatcherMock.Setup(x => x.Dispatch<DeleteCustomerCommand>(command)).Throws(new Exception("Test Message"));

            // Act
            var result = _controller.DeleteCustomer(command);
            
            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var objectResult = result as BadRequestObjectResult;
            Assert.AreEqual("Unable to delete customer. Test Message", objectResult.Value);
        }

    }
}