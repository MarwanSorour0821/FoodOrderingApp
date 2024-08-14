using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FoodApplication.Models;
using Application;
using Microsoft.EntityFrameworkCore;

namespace FoodApplication.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ForgotPasswordController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /ForgotPassword/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(forgotPasswordViewModel model)
        {
            // Query the employee table directly
            var employee = _context.employees.SingleOrDefault(e => e.Id == model.EmployeeID && e.Email == model.Email);

            if (employee != null)
            {
                // Generate 4-digit OTP
                var otp = new Random().Next(1000, 9999).ToString();

                // Store OTP and expiry time in session
                HttpContext.Session.SetString("OTP", otp);
                HttpContext.Session.SetString("OTPExpiry", DateTime.Now.AddMinutes(10).ToString());

                // Send OTP via email
                await SendOTPEmail(employee.Email, otp);

                // Redirect to OTP verification page
                return RedirectToAction("VerifyOTP");
            }

            // If employee is not found, return error
            ModelState.AddModelError("", "Invalid EmployeeID or Email.");
            return View("Index", model);
        }

        private async Task SendOTPEmail(string email, string otp)
        {
            var fromAddress = new MailAddress("thetechonomicpost@gmail.com", "foodApp");
            var toAddress = new MailAddress(email);
            const string fromPassword = "jtde diti xxal cmwn";
            const string subject = "Your OTP Code";
            string body = $"Your OTP code is {otp}. It is valid for 10 minutes.";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                await smtp.SendMailAsync(message);
            }
        }

        public IActionResult VerifyOTP()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyOTP(forgotPasswordViewModel model)
        {
            var storedOtp = HttpContext.Session.GetString("OTP");
            var expiryTime = HttpContext.Session.GetString("OTPExpiry");

            if (DateTime.TryParse(expiryTime, out DateTime otpExpiry) && DateTime.Now <= otpExpiry)
            {
                if (model.OTP == storedOtp)
                {
                    // OTP is correct, proceed to change password
                    HttpContext.Session.Remove("OTP");
                    HttpContext.Session.Remove("OTPExpiry");

                    // Redirect to change password view
                    return RedirectToAction("ChangePassword", "ForgotPassword");
                }
            }

            // OTP is incorrect or expired
            ModelState.AddModelError("", "Invalid or expired OTP.");
            return View("VerifyOTP", model);
        }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(forgotPasswordViewModel model)
        {

            var employee = _context.employees.Where(e => e.Id == model.EmployeeID);
            var user = await _context.users.Where(a => a.EmployeeID == model.EmployeeID).FirstOrDefaultAsync();

            if (employee != null && user != null)
            {
                    // TODO: Hash and set the new password
                    user.password = model.NewPassword; // Replace with hashed password
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    // Redirect to a confirmation page or login page
                    return RedirectToAction("Index", "Home");
            }

             ModelState.AddModelError("", "Invalid email address.");
            

             return View("Index", model);
        }


    }
}
