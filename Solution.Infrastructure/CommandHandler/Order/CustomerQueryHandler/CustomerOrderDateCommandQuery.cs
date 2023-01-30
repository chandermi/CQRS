namespace Solution.Infrastructure.CommandHandler.Order.CustomerQueryHandler
{
    public class CustomerOrderDateCommandQuery : IQuery<List<Solution.DAL.Order>>
    {
        public int customerId { get; set; }
        public string? OrderBy { get; set; }
    }
}
