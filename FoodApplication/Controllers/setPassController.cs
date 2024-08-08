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
    public class setPassController : Controller
    {

        //to get the id of the logged in user 
        
        

        private readonly ApplicationDbContext _context;
        


        public setPassController(ApplicationDbContext context)
        {
            _context = context;
        }

        


        // GET: /<controller>/
        public IActionResult Index(setPassViewModel model)
        {

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> SetPassword(setPassViewModel model)
        {

            int? firstTimeLogger = HttpContext.Session.GetInt32("firstTimeLogger");

            if (firstTimeLogger.HasValue)
            {
                var user = _context.users.SingleOrDefault(u => u.EmployeeID == firstTimeLogger.Value);

                if (user != null)
                {
                    user.password = model.newPassword;
                    user.isFirstLogin = false;
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home");
                }
            }


            return View(model);

        }

    }
}

