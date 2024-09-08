using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using FoodApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodApplication.Controllers
{
    public class foodController : Controller
    {
        private readonly ApplicationDbContext _context;

        public foodController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var model = new foodViewModel
            {
                FoodItems = _context.products.Where(a => a.productCategory.Equals("Food")).Select(a => new Product {
                       id = a.id,
                       productName = a.productName,
                       productInformation = a.productInformation,
                       productPrice = a.productPrice,
                       productImage = a.productImage,
                       productCategory = a.productCategory
                 }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(foodViewModel model)
        {

            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var foodItems = _context.products.Where(a => a.productCategory.Equals("Food")).ToList();

            //makes sure FoodItems is not null
            model.FoodItems = foodItems ?? new List<Product>();
            if (!foodItems.Any())
            {
                ModelState.AddModelError(string.Empty, "No food here");
            }

            return View(model);
        }


        [HttpPost]
        [Route("Desserts")]
        public IActionResult Desserts(foodViewModel model)
        {
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var foodItems = _context.products.Where(a => a.productCategory.Equals("Dessert")).ToList();

            //makes sure FoodItems is not null
            model.FoodItems = foodItems ?? new List<Product>();
            if (!foodItems.Any())
            {
                ModelState.AddModelError(string.Empty, "No food here");
            }

            return View(model);
        }

        [HttpPost]
        [Route("Drinks")]
        public IActionResult Drinks(foodViewModel model)
        {
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var foodItems = _context.products.Where(a => a.productCategory.Equals("Drink")).ToList();

            //makes sure FoodItems is not null
            model.FoodItems = foodItems ?? new List<Product>();
            if (!foodItems.Any())
            {
                ModelState.AddModelError(string.Empty, "No food here");
            }

            return View(model);
        }
    }
}
