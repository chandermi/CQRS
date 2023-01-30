using Microsoft.EntityFrameworkCore;
using Solution.DAL.Context;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.CommandHandler.Order.Command;
using Solution.Infrastructure.CommandHandler.Order.CustomerQueryHandler;
using Solution.Infrastructure.Repository;
using Solution.Infrastructure.Service;
using Solution.Infrastructure.UOW;

namespace Solution.UnitTest.Service
{
    [TestFixture]
    public class OrderServiceTests
    {
        private IUnitOfWork _unitOfWork;
        private IOrderRespository _orderService;
        private DbContextOptions<ApplicationDbContext> _options;
        [SetUp]
        public void SetUp()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "OrderServiceTests")
                .Options;
            var dbContext = new ApplicationDbContext(_options);
            _unitOfWork = new UnitOfWork(dbContext);
            _orderService = new OrderService(_unitOfWork);
        }
        [TearDown]
        public void TearDown()
        {
            var context = new ApplicationDbContext(_options);
            context.Orders.RemoveRange(context.Orders);
            context.SaveChanges();
        }

        [Test]
        public async Task Test_CreateOrder_Success()
        {
            //Arrange
            var command = new CreateOrderCommand
            {
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Items = new List<OrderItemCommand>
        {
            new OrderItemCommand { Product = new OrderItemProductCommand { Name = "Product 1", Price = 10 }, Quantity = 2 },
            new OrderItemCommand { Product = new OrderItemProductCommand { Name = "Product 2", Price = 20 }, Quantity = 3 },
            new OrderItemCommand { Product = new OrderItemProductCommand { Name = "Product 3", Price = 30 }, Quantity = 4 },
        }
            };

            //Act
            var result = await _orderService.CreateOrder(command);

            //Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_CreateOrder_Failed()
        {
            //Arrange
            var command = new CreateOrderCommand
            {
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Items = new List<OrderItemCommand>()
            };

            //Act
            var result = await _orderService.CreateOrder(command);

            //Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public async Task Test_GetOrderByDate_Success()
        {
            //Arrange
            var command = new CreateOrderCommand
            {
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Items = new List<OrderItemCommand>
        {
            new OrderItemCommand { Product = new OrderItemProductCommand { Name = "Product 1", Price = 10 }, Quantity = 2 },
            new OrderItemCommand { Product = new OrderItemProductCommand { Name = "Product 2", Price = 20 }, Quantity = 3 },
            new OrderItemCommand { Product = new OrderItemProductCommand { Name = "Product 3", Price = 30 }, Quantity = 4 },
        }
            };
            await _orderService.CreateOrder(command);

            var query = new CustomerOrderDateCommandQuery
            {
                customerId = 1,
                OrderBy = "asc"
            };

            //Act
            var result = await _orderService.GetOrderByDate(query);

            //Assert
            Assert.That(result.Count, Is.EqualTo(1));

        }

        [Test]
        public async Task Test_GetOrderByDate_desc_Success()
        {
            //Arrange
            var command = new CreateOrderCommand
            {
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Items = new List<OrderItemCommand>
        {
            new OrderItemCommand { Product = new OrderItemProductCommand { Name = "Product 1", Price = 10 }, Quantity = 2 },
            new OrderItemCommand { Product = new OrderItemProductCommand { Name = "Product 2", Price = 20 }, Quantity = 3 },
            new OrderItemCommand { Product = new OrderItemProductCommand { Name = "Product 3", Price = 30 }, Quantity = 4 },
        }
            };

            await _orderService.CreateOrder(command);
            command.OrderDate = DateTime.Now;
            await _orderService.CreateOrder(command);
            command.OrderDate = DateTime.Now;
            int LastOrderId = await _orderService.CreateOrder(command);

            var query = new CustomerOrderDateCommandQuery
            {
                customerId = 1,
                OrderBy = "desc"
            };

            //Act
            var result = await _orderService.GetOrderByDate(query);

            //Assert
            Assert.That(result.Count, Is.EqualTo(3));
            Assert.That(result[0].Id, Is.EqualTo(LastOrderId));

        }

        [Test]
        public async Task Test_GetAllOrder_By_Customer_Success()
        {
            //Arrange
            var command = new CreateOrderCommand
            {
                CustomerId = 1,
                OrderDate = DateTime.Now,
                Items = new List<OrderItemCommand>
        {
            new OrderItemCommand { Product = new OrderItemProductCommand { Name = "Product 1", Price = 10 }, Quantity = 2 },
            new OrderItemCommand { Product = new OrderItemProductCommand { Name = "Product 2", Price = 20 }, Quantity = 3 },
            new OrderItemCommand { Product = new OrderItemProductCommand { Name = "Product 3", Price = 30 }, Quantity = 4 },
        }
            };

            await _orderService.CreateOrder(command);
            command.OrderDate = DateTime.Now;
            await _orderService.CreateOrder(command);
            
            

            var query = new GetOrderQuery
            {
                customerId = 1,
            };

            //Act
            var result = await _orderService.GetOrders(query);

            //Assert
            Assert.That(result.Count, Is.EqualTo(2));
            

        }

    }
}