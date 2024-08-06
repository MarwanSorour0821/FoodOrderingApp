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
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private DateTime now;
        private Order currentOrder { get; set; }
        private OrderItem currentOrderItem { get; set; }

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult login1()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(userViewModel model)
        {

            if (ModelState.IsValid)
            {
                var validation = await _context.users
                    .Where(a => a.EmployeeID == model.EmployeeID && a.password == model.password)
                    .FirstOrDefaultAsync();

                if (validation != null)
                {
                    // Log or breakpoint here
                    Console.WriteLine("Login successful");
                    //currentOrder.userID = validation.EmployeeID;
                    //currentOrder.orderDate = TimeOnly.FromDateTime(now).ToString();
                     


                    return RedirectToAction(nameof(login1));
                }
                else
                {
                    // Log or breakpoint here
                    Console.WriteLine("Invalid login attempt");
                    ModelState.TryAddModelError(string.Empty, "Invalid login attempt");
                    return View("Index", model);
                }
            }

            return View("Index", model);
        }


    }
}

