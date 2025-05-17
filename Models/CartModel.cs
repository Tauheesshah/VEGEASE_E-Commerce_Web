using System.Text.Json.Serialization;

namespace VegEaseBackend.Models

{
    public class Cart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }

        public bool IsOrder { get; set; }
        [JsonIgnore]
        public virtual Product? Product { get; set; }

/*         public ICollection<OrderProductMapping> OrderProductMappings { get; set; }
*/    }

}

