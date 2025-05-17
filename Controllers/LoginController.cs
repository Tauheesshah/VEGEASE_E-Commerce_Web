using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using VegEaseBackend.Models;
using VegEaseBackend.Services;


namespace VegEaseBackend.Controllers
{
    public class LoginController : Controller
    {
        private readonly ProductDBContext _context;
        private readonly IEmailService _emailService;

        public LoginController(ProductDBContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View("~/Views/Login/Login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Username, Password")] Users user)
        {
            
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(x => x.Username == user.Username);

                if (existingUser == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    return View("Index");
                }

                if (user.Password != existingUser.Password)
                {
                    ModelState.AddModelError("", "Invalid password.");
                    return View("Index");
                }

                var userJson = JsonSerializer.Serialize(existingUser);
                HttpContext.Session.SetString("UserDetails", userJson);

                
                return RedirectToAction("Index", "Home");
                
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // SignUp Action
        public IActionResult SignUp()
        {
            return View("~/Views/Login/SignUp.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([Bind("Username, Password, PhoneNumber, Email, Address")] Users user)
        {
            if (ModelState.IsValid)
            {
                // Check if user already exists
                if (await _context.Users.AnyAsync(x => x.Username == user.Username))
                {
                    ModelState.AddModelError("", "User already exists.");
                    return View("SignUp");
                }

                // Add new user
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                await _emailService.SendEmailAsync(user.Username, user.Email, "Welcome to Raza Software and Technology");

                // Redirect to login page after successful registration
                return RedirectToAction("Index");
            }

            return View("SignUp");
        }
    }
}
