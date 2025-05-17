using System;
using System.Collections.Generic;
using VegEaseBackend.Models;

namespace VegEaseBackend.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public string Image { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Benefits { get; set; } = null!;

    public string Usage { get; set; } = null!;

    public virtual ICollection<Comment> Comments { get; set; }

/*    public virtual Category Category1 { get; set; } = null!;*/ 
    public virtual ICollection<Cart> Carts { get; set; } = new HashSet<Cart>();

}
