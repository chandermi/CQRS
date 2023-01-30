using Microsoft.EntityFrameworkCore;
using Solution.DAL;
using Solution.DAL.Context;
using Solution.Infrastructure.CommandHandler.Customer.CreateCommand;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.CommandHandler.Customer.DeleteCommand;
using Solution.Infrastructure.CommandHandler.Customer.UpdateCommand;
using Solution.Infrastructure.Repository;
using Solution.Infrastructure.Service;
using Solution.Infrastructure.UOW;

namespace Solution.UnitTest.Service
{
    public class CustomerServiceTests
    {
        private ICustomerRespository _customerService;
        private IUnitOfWork _unitOfWork;
        private DbContextOptions<ApplicationDbContext> _options;

        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            var context = new ApplicationDbContext(_options);
            _unitOfWork = new UnitOfWork(context);
            _customerService = new CustomerService(_unitOfWork);
        }

        [TearDown]
        public void TearDown()
        {
            var context = new ApplicationDbContext(_options);
            context.Customers.RemoveRange(context.Customers);
            context.SaveChanges();
        }

        [Test]
        public async Task CreateCustomer_ValidInput_ReturnsCustomerId()
        {
            // Arrange
            var cmd = new CreateCustomerCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                PostalCode = "12345"
            };

            // Act
            var customerId = await _customerService.CreateCustomer(cmd);

            // Assert
            Assert.That(customerId, Is.Not.EqualTo(0));
            var customer = _unitOfWork.Customers.Find(customerId);
            Assert.That(customer.FirstName, Is.EqualTo("John"));
            Assert.That(customer.LastName, Is.EqualTo("Doe"));
            Assert.That(customer.Address, Is.EqualTo("123 Main St"));
            Assert.That(customer.PostalCode, Is.EqualTo("12345"));
        }

        [Test]
        public async Task DeleteCustomer_ValidInput_ReturnsTrue()
        {
            // Arrange
            var cmd = new CreateCustomerCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Address = "123 Main St",
                PostalCode = "12345"
            };
            var customerId = await _customerService.CreateCustomer(cmd);

            // Act
            var deleteCmd = new DeleteCustomerCommand { Id = customerId };
            var result = await _customerService.DeleteCustomer(deleteCmd);

            // Assert
            Assert.IsTrue(result);
            var customer = _unitOfWork.Customers.Find(customerId);
            Assert.IsNull(customer);
        }

        [Test]
        public async Task DeleteCustomer_InValidInput_ReturnsTrue()
        {
            // Arrange
            var customerId = 0;

            // Act
            var deleteCmd = new DeleteCustomerCommand { Id = customerId };
            var result = await _customerService.DeleteCustomer(deleteCmd);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetCustomerById_ValidId_ReturnsCustomer()
        {
            // Arrange
            var customer = new Customer("John", "Doe", "123 Main St", "12345");
            _unitOfWork.Customers.Add(customer);
            _unitOfWork.SaveChanges();

            var query = new GetCustomerQuery { Id = customer.Id };

            // Act
            var result = await _customerService.GetCustomerById(query);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(customer.Id));
            Assert.That(result.FirstName, Is.EqualTo(customer.FirstName));
            Assert.That(result.LastName, Is.EqualTo(customer.LastName));
            Assert.That(result.Address, Is.EqualTo(customer.Address));
            Assert.That(result.PostalCode, Is.EqualTo(customer.PostalCode));
        }

        [Test]
        public Task GetCustomerById_InvalidId_ThrowsNullReferenceException()
        {
            // Arrange
            var query = new GetCustomerQuery { Id = 99 };

            // Act and Assert
            var ex = Assert.ThrowsAsync<ArgumentNullException>(() => _customerService.GetCustomerById(query));
            return Task.CompletedTask;
        }

        [Test]
        public async Task UpdateCustomer_UpdatesCorrectCustomer()
        {
            // Arrange
            var customer = new Customer("John", "Doe", "123 Main St", "12345");
            _unitOfWork.Customers.Add(customer);
            _unitOfWork.SaveChanges();
            int customerId = customer.Id;
            var query = new GetCustomerQuery { Id = customer.Id };

            // Act

            UpdateCustomerCommand command = new UpdateCustomerCommand()
            {

                Address = $"{customer.Address} Update",
                LastName = $"{customer.LastName} Update",
                FirstName = $"{customer.FirstName} Update",
                Id = customerId,
                PostalCode = $"{customer.PostalCode} Update",
            };
            await _customerService.UpdateCustomer(command);

            var result = await _customerService.GetCustomerById(query);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(command.Id));
            Assert.That(result.FirstName, Is.EqualTo(command.FirstName));
            Assert.That(result.LastName, Is.EqualTo(command.LastName));
            Assert.That(result.Address, Is.EqualTo(command.Address));
            Assert.That(result.PostalCode, Is.EqualTo(command.PostalCode));
        }

    }
}
