namespace Solution.DAL
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        
        public decimal TotalPrice { get; set; }
        public List<Item>? Items { get; set; }

    }
}
