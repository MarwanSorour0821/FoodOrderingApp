﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using FoodApplication.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodApplication.Controllers
{
    public class dessertController : Controller
    {

        private readonly ApplicationDbContext _context;

        public dessertController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var model = new dessertViewModel
            {
                dessertItems = _context.products.Where(a => a.productCategory.Equals("Dessert")).Select(a => new Product
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
        public async Task<IActionResult> Index(dessertViewModel model)
        {
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var dessertItems = _context.products.Where(a => a.productCategory.Equals("Dessert")).ToList();

            //makes sure FoodItems is not null
            model.dessertItems = dessertItems ?? new List<Product>();
            if (!dessertItems.Any())
            {
                ModelState.AddModelError(string.Empty, "No drinks available now");
            }

            return View(model);
        }



    }
    
}

