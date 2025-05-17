using Microsoft.AspNetCore.Mvc;

namespace VegEaseBackend.Controllers
{
    public class CartViewController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Products/cart.cshtml");
        }
    }
}
