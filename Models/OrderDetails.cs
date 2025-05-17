namespace VegEaseBackend.Models
{
    public class OrderDetails
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public int UserId { get; set; }

        // Navigation Property
        public Users User { get; set; }
        public ICollection<OrderProductMapping> OrderProductMappings { get; set; }
    }
}
