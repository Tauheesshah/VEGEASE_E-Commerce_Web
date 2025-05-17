namespace VegEaseBackend.Models
{
    public class OrderProductMapping
    {
        public int OPId { get; set; }

        public int CartId { get; set; }
        public int OrderId { get; set; }

        // Navigation Properties
        public Cart Cart { get; set; }
        public OrderDetails OrderDetails { get; set; }
    }
}
