using Microsoft.AspNetCore.Mvc;
using VegEaseBackend.Models;
using System.Linq;

namespace VegEaseBackend.Controllers
{
    [Route("Admin/[action]")]
    public class AdminController : Controller
    {
        private readonly ProductDBContext _context;

        public AdminController(ProductDBContext context)
        {
            _context = context;
        }

      
        public IActionResult AdminPage()
        {
            return View("~/Views/AddAdmin/AddProduct.cshtml");
        }

        // POST: Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _context.Admins
                    .FirstOrDefault(a => a.UserName == model.UserName && a.Password == model.Password);

                if (admin != null)
                {
                    return RedirectToAction("AdminPage", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            }
            return View(model);
        }

        // GET: Admin/Logout (Optional if needed)
        public IActionResult Logout()
        {
            // Since we are not using session, Logout might be redundant
            // Redirect to Login page
            return RedirectToAction("Login");
        }
    }
}
