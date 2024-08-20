using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using FoodApplication.Models;
using Microsoft.AspNetCore.Mvc;
using static System.Collections.Specialized.BitVector32;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FoodApplication.Controllers
{
    public class RemoveController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RemoveController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RemoveProduct
        public IActionResult Index(string searchString)
        {
            var products = from p in _context.products
                           select p;

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.productName.Contains(searchString));
            }

            return View(products.ToList());
        }

        // POST: RemoveProduct/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var product = _context.products.Find(id);
            if (product != null)
            {
                _context.products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, creatViewModel model)
        {
            id = (int) HttpContext.Session.GetInt32("productID");
            var product = _context.products.Find(id);
            if (product != null)
            {
                product.id = id;
                product.productCategory = model.productCategory;
                product.productImage = model.productInformation;
                product.productName = model.productName;
                product.productPrice = model.productPrice;
                product.productStock = model.productStock;

                _context.products.Update(product);
                _context.SaveChanges();

            }

            return RedirectToAction(nameof(Index));
        }


        public IActionResult adjust(int id)
        {
            HttpContext.Session.SetInt32("productID", id);

            return View();
        }
            

        // GET: /<controller>/
        //public IActionResult Index()
        //{
        //    return View();
        //}




    }
}

