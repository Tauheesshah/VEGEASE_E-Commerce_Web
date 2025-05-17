using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VegEaseBackend.Models;

namespace VegEaseBackend.Controllers
{
    public class CategoryController : Controller
    {
        [HttpGet("Category/Index")]
        public IActionResult Index(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                return NotFound();
            }

            ViewBag.CategoryType = type;
            return View("~/Views/Products/Category.cshtml");
        }
    }
}


/*Category objCat = new Category();
objCat.categoryName = category;
var products = await _context.Products
    .Where(p => p.Category == category)
    .ToListAsync();
objCat.products = products;
return Ok(objCat);*//*


using Microsoft.AspNetCore.Mvc;
using VegEaseBackend.Models;
using System.Linq;

namespace VegEaseBackend.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ProductDBContext _context;

        public CategoryController(ProductDBContext context)
        {
            _context = context;
        }

        public IActionResult CategoryTypes(string type)
        {
            Category objCat = new Category();

            objCat.CategoryType = type;
            objCat.Products= _context.Products
                    .Where(p => p.Category == type)
                    .ToList();
            return View("~/Views/Products/Category.cshtml", objCat);
        }

    }
}*/
