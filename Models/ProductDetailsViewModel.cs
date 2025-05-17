using System.Collections.Generic;

namespace VegEaseBackend.Models
{
    public class ProductDetailsViewModel
    {
        public Product Product { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
