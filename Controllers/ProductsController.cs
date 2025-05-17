using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VegEaseBackend.Models;

namespace VegEaseBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ProductDBContext _context;

        public ProductsController(ProductDBContext context)
        {
            _context = context;
        }

        [HttpGet("{category}")]
        public async Task<IActionResult> GetProduct(string category)
        {

            var products = await _context.Products
                .Where(p => p.Category == category)
                .ToListAsync();
            return Ok(products);
        }



        [HttpGet("product/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Benefits,
                    p.Usage,
                    p.Image
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        [HttpGet("{id}/comments")]
        public async Task<IActionResult> GetCommentsForProduct(int id)
        {
            var comments = await _context.Comments
                .Where(c => c.PId == id)
                .Select(c => new
                {
                    c.CommentText,
                    c.Rating,
                    c.CreatedAt
                })
                .ToListAsync();

            return Ok(comments);
        }

        [HttpPost("{id}/comments")]
        public async Task<IActionResult> AddComment(int id, [FromBody] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.PId = id;
                comment.CreatedAt = DateTime.UtcNow;

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                return Ok(comment);
            }

            return BadRequest(ModelState);
        }



    }
}
