using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Application;
using FoodApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FoodApplication.Controllers
{
    public class searchOrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public searchOrderController(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        // GET: /<controller>/
        [HttpGet]
        [Route("IndexSearch")]
        public IActionResult Index(string searchString)
        {
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var orders = _context.orders;

            if (!string.IsNullOrEmpty(searchString))
            {
                var orderss = orders.Where(o => o.orderNumber.ToString().Contains(searchString) ||
                                           o.userID.ToString().Contains(searchString) ||
                                           o.status.Contains(searchString));
                return View(orderss);
                
            }

            var empList = orders.ToList().OrderByDescending(o => o.Id);
            foreach (var emp in empList)
            {
                _context.Entry(emp).Reload();
            }
            return View(empList);

           
        }

        [HttpPost]
        public IActionResult UpdateStatus(int orderID)
        {
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var order = _context.orders.Find(orderID);
            if (order != null)
            {
                order.status = "Delivered";
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("SendNotification")]
        public async Task<IActionResult> SendNotification(int orderId)
        {
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var order = await _context.orders.FindAsync(orderId);
            var userID = order.userID;

            var user = await _context.users.FindAsync(userID);


            if (order == null)
            {
                return Json(new { success = false, message = "Invalid order or email." });
            }

            try
            {
                // Assuming SendEmailAsync is a method that sends an email
                await SendOTPEmail(user.Email, order.Id);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Route("GenerateQrCode")]
        public IActionResult GenerateQrCode(int orderID)
        {
            var order = _context.orders.Find(orderID);

            string qrCodeUrl = $"https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=Order{order.orderNumber}";
            return Redirect(qrCodeUrl);
        }

        private async Task SendOTPEmail(string email, int orderID)
        {
            var order = await _context.orders.FindAsync(orderID);
            var fromAddress = new MailAddress("thetechonomicpost@gmail.com", "foodApp");
            var toAddress = new MailAddress(email);
            const string fromPassword = "jtde diti xxal cmwn";
            const string subject = "Order Ready to be Collected!";
            string body = $"Dear Customer, Your order is ready for collection. Your order number is {order.orderNumber}.";

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


    }
}
