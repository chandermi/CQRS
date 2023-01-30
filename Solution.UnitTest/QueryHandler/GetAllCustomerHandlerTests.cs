using Solution.Infrastructure.CommandHandler.Customer.CreateCommand;
using Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler;
using Solution.Infrastructure.CommandHandler.Customer.DeleteCommand;
using Solution.Infrastructure.CommandHandler.Customer.UpdateCommand;
using Solution.Infrastructure.Repository;

namespace Solution.UnitTest.GetAllCustomerHandlerTests
{
    [TestFixture]
    public class GetAllCustomerHandlerTests
    {
        private GetAllCustomerHandler _handler;
        private ICustomerRespository _mockRepo;

        [SetUp]
        public void SetUp()
        {
            _mockRepo = new MockCustomerRepository();
            _handler = new GetAllCustomerHandler(_mockRepo);
        }

        [Test]
        public async Task HandleAsync_Should_Return_All_Customers()
        {
            //Arrange
            var query = new GetAllCustomerQuery();

            //Act
            var result = await _handler.HandleAsync(query);

            //Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("John", result[0].FirstName);
            Assert.AreEqual("Doe", result[0].LastName);
            Assert.AreEqual("Jane", result[1].FirstName);
            Assert.AreEqual("Doe", result[1].LastName);
            Assert.AreEqual("Bob", result[2].FirstName);
            Assert.AreEqual("Smith", result[2].LastName);
        }
    }

    public class MockCustomerRepository : ICustomerRespository
    {
        public Task<int> CreateCustomer(CreateCustomerCommand cmd)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> DeleteCustomer(DeleteCustomerCommand cmd)
        {
            throw new System.NotImplementedException();
        }

        public Task<Solution.DAL.Customer> GetCustomerById(GetCustomerQuery query)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<Solution.DAL.Customer>> GetCustomers(GetAllCustomerQuery query)
        {
            return Task.FromResult(new List<Solution.DAL.Customer>
        {
            new Solution.DAL.Customer("John","Doe","",""),
            new Solution.DAL.Customer ("Jane","Doe","",""),
            new Solution.DAL.Customer ("Bob","Smith","","")
        });
        }

        public Task<bool> UpdateCustomer(UpdateCustomerCommand cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}