using System.Text;
using Application;
using FoodApplication.Models;
using Microsoft.AspNetCore.Authentication;
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
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }


        public IActionResult login1(foodViewModel model)
        {
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var foodItems = _context.products.Where(a => a.productCategory.Equals("Food")).ToList();

            //makes sure FoodItems is not null
            model.FoodItems = foodItems ?? new List<Product>();
            if (!foodItems.Any())
            {
                ModelState.AddModelError(string.Empty, "No food here");
            }

            return View(model);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(userViewModel model)
        //{
        //    var validation = await _context.users
        //            .Where(a => a.EmployeeID == model.EmployeeID && a.password == model.password)
        //            .FirstOrDefaultAsync();

        //    if (validation == null)
        //    {
        //        Console.WriteLine("Invalid login attempt");
        //        ModelState.TryAddModelError(string.Empty, "Invalid login attempt");
        //        return View("Index", model);
        //    }

        //    if (validation.isFirstLogin == true)
        //    {
        //        HttpContext.Session.SetInt32("firstTimeLogger", validation.EmployeeID);
        //        HttpContext.Session.SetString("EMAIL", validation.Email);
        //        HttpContext.Session.SetInt32("ID", validation.Id);
        //        return RedirectToAction("Index", "setPass");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        HttpContext.Session.SetInt32("UserRole", validation.RoleID);
        //        HttpContext.Session.SetString("EMAIL", validation.Email);
        //        HttpContext.Session.SetInt32("ID", validation.Id);
        //        return RedirectToAction(nameof(login1));
        //    }

        //    HttpContext.Session.SetString("EMAIL", validation.Email);

        //    return View("Index", model);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(userViewModel model)
        {
            // Fetch the user based on the EmployeeID
            var user = await _context.users
                .Where(a => a.EmployeeID == model.EmployeeID)
                .FirstOrDefaultAsync();

            // If the user is not found, display an error
            if (user == null)
            {
                Console.WriteLine("Invalid login attempt");
                ModelState.TryAddModelError(string.Empty, "Invalid login attempt");
                return View("Index", model);
            }

            // Check if it's the user's first login
            if (user.isFirstLogin)
            {
                HttpContext.Session.SetInt32("firstTimeLogger", user.EmployeeID);
                HttpContext.Session.SetString("EMAIL", user.Email);
                HttpContext.Session.SetInt32("ID", user.Id);
                return RedirectToAction("Index", "setPass");
            }

            // Decrypt the stored password
            var decryptedPassword = Decrypt(user.password);

            // Compare the decrypted password with the input password
            if (decryptedPassword != model.password)
            {
                Console.WriteLine("Invalid login attempt");
                ModelState.TryAddModelError(string.Empty, "Invalid login attempt");
                return View("Index", model);
            }

            if (ModelState.IsValid)
            {
                HttpContext.Session.SetInt32("UserRole", user.RoleID);
                HttpContext.Session.SetString("EMAIL", user.Email);
                HttpContext.Session.SetInt32("ID", user.Id);
                return RedirectToAction(nameof(login1));
            }

            HttpContext.Session.SetString("EMAIL", user.Email);

            return View("Index", model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Logout()
        {
            HttpContext.Session.Clear();

            // Sign out the user if using ASP.NET Core Identity or Cookie Authentication
            //await HttpContext.SignOutAsync();

            // Redirect to the login page or home page after logout
            return RedirectToAction("Index", "Login");
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

