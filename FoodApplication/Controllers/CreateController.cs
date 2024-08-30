using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using FoodApplication.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodApplication.Controllers
{
    public class CreateController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly string _imagesPath;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CreateController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = Path.Combine(webHostEnvironment.WebRootPath, "images");
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(creatViewModel model)
        {
            if (ModelState.IsValid)
            {
                string imageFileName = null;

                if (model.Image != null && model.Image.Length > 0)
                {
                    imageFileName = await SaveCover(model.Image);
                }
                var lastProduct = _context.products.OrderByDescending(p => p.id).FirstOrDefault();
                int lastProductId = lastProduct?.id ?? 0;

                var product = new Product()
                    {
                        id = lastProductId + 1,
                        productName = model.productName,
                        productCategory = model.productCategory,
                        productImage = imageFileName,
                        productInformation = model.productInformation,
                        productPrice = model.productPrice,
                        productStock = model.productStock
                    };

                _context.products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Admin1", "Home");

            }

            return View(model);




        }




        private async Task<string> SaveCover(IFormFile cover)
        {
            if (cover == null || cover.Length == 0)
                return null;

            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var path = Path.Combine(_imagesPath, coverName);

            if (!Directory.Exists(_imagesPath))
            {
                Directory.CreateDirectory(_imagesPath);
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await cover.CopyToAsync(stream);
            }

            return coverName; // Return the file name or path for storage in the database
        }

    }


}

