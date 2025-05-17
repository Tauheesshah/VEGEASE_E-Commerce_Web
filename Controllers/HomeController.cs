using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VegEaseBackend.Models;

namespace VegEaseBackend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Fruits()
        {
            ViewBag.CategoryType = "Fresh-Fruits";
            return View("~/Views/Products/Category.cshtml");
        }
        public IActionResult Vegetables()
        {
            ViewBag.CategoryType = "Fresh-Vegetables";
            return View("~/Views/Products/Category.cshtml");
        }

        
        public IActionResult OrganicProducts()
        {
            ViewBag.CategoryType = "Leaf-Herbs";
            return View("~/Views/Products/Category.cshtml");
        }

       
        public IActionResult ExoticFruits()
        {
            ViewBag.CategoryType = "Exotic-Fruits";
            return View("~/Views/Products/Category.cshtml");
        }

       
        public IActionResult ExoticVegetables()
        {
            ViewBag.CategoryType = "Exotic-Vegetables";
            return View("~/Views/Products/Category.cshtml");
        }

       
        public IActionResult AboutUs()
        {
            return View("~/Views/Profile_items/AboutUs.cshtml");
        }

       
        public IActionResult ContactUs()
        {
            return View("~/Views/Profile_items/ContactUs.cshtml");
        }
    }


}
