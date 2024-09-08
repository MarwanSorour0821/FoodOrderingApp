using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using FoodApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace FoodApplication.Controllers
{
    public class setPassController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public setPassController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _passwordHasher = passwordHasher;
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
                    //check if confirm password and new password are equal, if not return the current page

                    //TODO: HASH PASSWORD HERE 
                    if (model.newPassword == model.confirmPassword)
                    {
                        //user.password = model.newPassword;
                        user.password = Encrypt(model.newPassword);
                        user.isFirstLogin = false;
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "setPass");
                    }
                    
                }
            }
            return View(model);
        }



        //FOR PASSWORD ENCRYPTION/DECRYPTION    
        private string Encrypt(string password)

        {

            if (string.IsNullOrEmpty(password))

                return null;

            else

            {

                byte[] storePassword = ASCIIEncoding.ASCII.GetBytes(password);

                string encryptedPassword = Convert.ToBase64String(storePassword);

                return encryptedPassword;

            }

        }

        private string Decrypt(string password)

        {

            if (string.IsNullOrEmpty(password))

            {

                return null;

            }

            else

            {

                byte[] encryptedPassword = Convert.FromBase64String(password);

                string decryptedPassword = ASCIIEncoding.ASCII.GetString(encryptedPassword);

                return decryptedPassword;

            }

        }


    }
}

