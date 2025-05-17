using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VegEaseBackend.Models;
using System.Linq;
using System.Threading.Tasks;

namespace VegEaseBackend.Controllers
{
    public class ProductMvcController : Controller
    {
        private readonly ProductDBContext _context;

        public ProductMvcController(ProductDBContext context)
        {
            _context = context;
        }

        [Route("product-details")]
        public async Task<IActionResult> ProductDetails(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View("~/Views/Products/ProductDetails.cshtml", product);
        }
    }
}
