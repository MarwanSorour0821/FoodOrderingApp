using Application;
using FoodApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FoodApplication.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        
        public IActionResult det(int orderId)
        {
            var order = _context.orders.FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
