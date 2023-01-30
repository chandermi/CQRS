using Moq;
using Solution.Infrastructure.CommandHandler.Order.Command;
using Solution.Infrastructure.CommandHandler.Order.CreateCommand;
using Solution.Infrastructure.Repository;

namespace Solution.UnitTest.CommandHandler
{
    [TestFixture]
    public class CreateOrderCommandTest
    {
        private OrderCommandHandler _orderCommandHandler;
        private Mock<IOrderRespository> _mockOrderRepository;

        [SetUp]
        public void SetUp()
        {
            _mockOrderRepository = new Mock<IOrderRespository>();
            _orderCommandHandler = new OrderCommandHandler(_mockOrderRepository.Object);
        }

        [Test]
        public void Handle_GivenCreateOrderCommand_CallsCreateOrderOnRepository()
        {
            //Arrange
            var createOrderCommand = new CreateOrderCommand
            {
                CustomerId = 1,
                OrderDate = DateTime.Now,
                TotalPrice = 100,
                Items = new List<OrderItemCommand>
            {
                new OrderItemCommand
                {
                    Product = new OrderItemProductCommand
                    {
                        Name = "Product 1",
                        Price = 50
                    },
                    Quantity = 2
                }
            }
            };

            //Act
            _orderCommandHandler.Handle(createOrderCommand);

            //Assert
            _mockOrderRepository.Verify(x => x.CreateOrder(createOrderCommand), Times.Once);
        }
    }
}
