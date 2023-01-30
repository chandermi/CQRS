namespace Solution.Infrastructure.CommandHandler.Order.Command
{
    public class CreateOrderCommand : ICommandDisp
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemCommand> Items { get; set; }
    }

}
