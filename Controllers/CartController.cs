using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using VegEaseBackend.Models;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ProductDBContext _context;

    public CartController(ProductDBContext context)
    {
        _context = context;
    }
    

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] Cart cartItem)
    {
        var userJson = HttpContext.Session.GetString("UserDetails");
        if (userJson != null)
        {
            var user = JsonSerializer.Deserialize<Users>(userJson);
            // Use the user object as needed
        }
        if (cartItem == null)
        {
            return BadRequest("Invalid cart data.");
        }

        // Check if the ProductId exists in the Products table
        var product = await _context.Products.FindAsync(cartItem.ProductId);
        if (product == null)
        {
            return NotFound("Product not found.");
        }

        // Optionally: Check if the item already exists in the cart and update quantity if needed
        var existingCartItem = await _context.Carts
                                            .FirstOrDefaultAsync(c => c.ProductId == cartItem.ProductId && c.IsOrder == false);
        if (existingCartItem != null)
        {
            existingCartItem.Quantity += cartItem.Quantity;
            _context.Carts.Update(existingCartItem);
        }
        else
        {
            _context.Carts.Add(cartItem);
        }

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { message = "Product added to cart successfully." });
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception occurred while adding product to cart: " + ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpGet("items")]
    public async Task<IActionResult> GetCartItems()
        {
        try
        {
            var cartItems = await _context.Carts.Where(c => c.IsOrder == false)
                                          .Include(c => c.Product) 
                                          .ToListAsync();

            if (cartItems == null || !cartItems.Any())
            {
                return NotFound("No items found in the cart.");
            }

            return Ok(cartItems);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception occurred while retrieving cart items: " + ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }

    [HttpDelete("remove/{id}")]
    public async Task<IActionResult> RemoveFromCart(int id)
    {
        var cartItem = await _context.Carts.FindAsync(id);
        if (cartItem == null)
        {
            return NotFound("Cart item not found.");
        }

        _context.Carts.Remove(cartItem);

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { message = "Item removed from cart successfully." });
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception occurred while removing item from cart: " + ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }


    [HttpPut("update")]
    public async Task<IActionResult> UpdateQuantity(int ProductId, string mode)
    {
        Console.WriteLine($"ProductId: {ProductId}, Mode: {mode}");
        var cartItem = _context.Carts.FirstOrDefault(x=>x.ProductId == ProductId && x.IsOrder == false);
        if (cartItem == null)
        {
            return NotFound("Cart item not found.");
        }

        if (mode.ToLower() == "a")
        {
             cartItem.Quantity += 1;
            _context.Carts.Update(cartItem);
        }
        else
        {
            cartItem.Quantity -= 1;
            if(cartItem.Quantity <= 0)
            {
                _context.Carts.Remove(cartItem);
            }
            else
            {
            _context.Carts.Update(cartItem);
            }
        }

        try
        {
            await _context.SaveChangesAsync();
            return Ok(new { message = "Quantity updated successfully." });
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Exception occurred while updating quantity: " + ex.Message);
            return StatusCode(500, "Internal server error.");
        }
    }

}
