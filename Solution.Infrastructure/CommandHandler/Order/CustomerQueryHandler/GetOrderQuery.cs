namespace Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler
{
    public class GetOrderQuery : IQuery<List<Solution.DAL.Order>>
    {
        public int customerId { get; set; }
    }
}
