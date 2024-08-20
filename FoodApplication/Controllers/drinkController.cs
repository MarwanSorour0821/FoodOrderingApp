using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using FoodApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodApplication.Controllers
{
    public class drinkController : Controller
    {
        private readonly ApplicationDbContext _context;

        public drinkController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            var model = new drinkViewModel
            {
                drinkItems = _context.products.Where(a => a.productCategory.Equals("Drink")).Select(a => new Product
                {
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
        public async Task<IActionResult> Index(drinkViewModel model)
        {
            var DrinkItems = _context.products.Where(a => a.productCategory.Equals("Drink")).ToList();

            //makes sure FoodItems is not null
            model.drinkItems = DrinkItems ?? new List<Product>();
            if (!DrinkItems.Any())
            {
                ModelState.AddModelError(string.Empty, "No drinks available now");
            }

            return View(model);
        }



    }
}

