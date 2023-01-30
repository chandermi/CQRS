namespace Solution.Infrastructure.CommandHandler.Order.Command
{
    public class OrderItemCommand : ICommandDisp
    {

        public OrderItemProductCommand Product { get; set; }
        public int Quantity { get; set; }
    }

}
