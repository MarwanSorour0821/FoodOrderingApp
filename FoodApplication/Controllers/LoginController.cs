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
            var validation = await _context.users
                    .Where(a => a.EmployeeID == model.EmployeeID && a.password == model.password)
                    .FirstOrDefaultAsync();

            //TODO: DEAL WITH NULL EXCEPTION HERE
            if (validation.isFirstLogin == true)
            {
                HttpContext.Session.SetInt32("firstTimeLogger", validation.EmployeeID);
                return RedirectToAction("Index", "setPass");
            }

            if (ModelState.IsValid)
            {
                if (validation != null)
                {
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

