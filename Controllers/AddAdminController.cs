using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using VegEaseBackend.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace VegEaseBackend.Controllers
{
    public class AddAdminController : Controller
    {
        private readonly ProductDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AddAdminController(ProductDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: /Admin/AddProduct
        public IActionResult AddProduct()
        {
            return View();
        }

        // POST: /Admin/AddProduct
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product model, IFormFile ImageFile)
        {
            try
            {
                // Check if an image file was uploaded
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    // Define the path to save the image in the images folder inside wwwroot
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Create the directory if it doesn't exist
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Save the image file to the defined path
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    // Set the image path in the model (relative path to be stored in the database)
                    model.Image =  uniqueFileName;
                }

                // Save the product details to the database
                _context.Products.Add(model);
                await _context.SaveChangesAsync();
                
                // Redirect to admin home page or other appropriate page after adding the product
                return RedirectToAction("Admin_pannel_Home");
            }
            catch (Exception ex)
            {
                // Log the error message
                ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            }

            // Return the same view with the model to display any validation errors
            return View(model);
        }

    }
}
