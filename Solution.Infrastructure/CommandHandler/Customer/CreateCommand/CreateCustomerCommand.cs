namespace Solution.Infrastructure.CommandHandler.Customer.CreateCommand
{
    public class CreateCustomerCommand : ICommandDisp
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

    }
}
