namespace Solution.DAL
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        private Customer() { }

        public Customer(string firstName, string lastName,string address,string  postalCode)
        {
            FirstName = firstName;
            LastName = lastName;
            Address = address;
            PostalCode= postalCode;
        }

    }

}
