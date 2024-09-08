using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using FoodApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            //if (HttpContext.Session.GetString("EMAIL") == null)
            //{
            //    return RedirectToAction("Index", "Login");
            //}

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
            //var product = _context.products.Find(id);
            //if (product != null)
            //{
            //    _context.products.Remove(product);
            //    _context.SaveChanges();
            //}
            //return RedirectToAction(nameof(Index));

            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            try
            {
                var product = _context.products.Find(id);
                if (product != null)
                {
                    _context.products.Remove(product);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException ex)
            {
                // Log the error or inspect ex.InnerException for details
                var innerException = ex.InnerException?.Message;
                // You can also rethrow or handle the exception differently
                return Json(new { success = false, message = $"Could not add product to cart: {innerException}" });
            }

            return Json(new { success = true, message = "Added" });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, creatViewModel model)
        {
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

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
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

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

