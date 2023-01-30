namespace Solution.DAL
{
    public class Item
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        private Item() { }

        public Item(Product product, int quantity)
        {

            Product = product;
            Quantity = quantity;
        }
        public Item(int id,int orderId,int productId, Product product, int quantity)
        {
            Id = id;
            OrderId = orderId;
            ProductId = productId;
            Product = product;
            Quantity = quantity;
        }
    }
}
