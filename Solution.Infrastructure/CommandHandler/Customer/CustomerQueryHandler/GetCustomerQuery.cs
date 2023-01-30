namespace Solution.Infrastructure.CommandHandler.Customer.CustomerQueryHandler
{
    public class GetCustomerQuery : IQuery<Solution.DAL.Customer>
    {
        public int Id { get; set; }
    }
}
