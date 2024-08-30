using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using FoodApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodApplication.Controllers
{
    public class searchOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public searchOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        public IActionResult Index(string searchString)
        {
            var orders = _context.orders;

            if (!string.IsNullOrEmpty(searchString))
            {
                var orderss = orders.Where(o => o.Id.ToString().Contains(searchString) ||
                                           o.userID.ToString().Contains(searchString) ||
                                           o.status.Contains(searchString));
                return View(orderss.ToList());
            }

            return View(orders.ToList());
        }

        [HttpPost]
        public IActionResult UpdateStatus(int orderID)
        {
            var order = _context.orders.Find(orderID);
            if (order != null)
            {
                order.status = "Delivered";
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        
    }
}
