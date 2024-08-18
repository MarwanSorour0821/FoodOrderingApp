using System.Linq;
using System.Threading.Tasks;
using FoodApplication.Models;
using Microsoft.AspNetCore.Mvc;
using FoodApplication.Helper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Application;
using QRCoder;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Mail;
using ZXing;
using System.Drawing; 
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Imaging;
using static QRCoder.PayloadGenerator;

namespace FoodApplication.Controllers
{
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "CartItems";
        private const string orderSession = "Orders";
        

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("AddProductToCartAsync")]
        public async Task<IActionResult> AddProductToCartAsync(int? id, int quantity)
        {
            //var product = await _context.products.Where(u => u.productName == model.productName).FirstOrDefaultAsync();
            var product = await _context.products.FindAsync(id);

            Order newOrder = new Order();

            //ADD order to database


            if (product == null)
            {
                return Json(new { success = false, message = "Product Not Found" });
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            var order = HttpContext.Session.GetObjectFromJson<Order>(orderSession) ?? new Order
            {
                Id = newOrder.Id,
                orderDate = DateTime.Now.ToString("yyyy-MM-dd"),
                status = "Pending",
                orderItems = new List<OrderItem>()
            };

            var existingProduct = cart.Find(c => c.productID == id);

            //Order order = new Order();

            //_context.orders.Add(order);
            //_context.SaveChanges();




            //if (existingProduct.itemQuantity == 0)
            //{
            //    return Json(new { success = false, message = "Cannot add item with quantity 0" });
            //}

            if (existingProduct != null)
            {
                existingProduct.itemQuantity++;
            }
            else if (product.productStock < quantity)
            {
                return Json(new { success = false, message = "Quantity more than stock" });
            }
            else
            {
                cart.Add(new OrderItem
                {
                    orderID = order.Id,
                    productID = product.id,
                    itemQuantity = quantity,
                    productPrice = product.productPrice
                });

                
            }
            HttpContext.Session.SetObjectAsJson(orderSession, order);



            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);
            return Json(new { success = true });
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            return View(cart);
        }

        [HttpGet]
        [Route("TotalPrice")]
        public IActionResult GetTotalPrice()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            var totalPrice = cart.Sum(item => item.itemQuantity * item.productPrice);
            return Json(new { totalPrice });
        }

        [HttpGet]
        [Route("ItemPrice/{id}")]
        public IActionResult Price(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            var item = cart.FirstOrDefault(i => i.productID == id);
            if (item == null)
            {
                return Json(new { success = false, message = "Product not found in cart" });
            }

            var totalItemPrice = item.itemQuantity * item.product.productPrice;
            return Json(new { totalItemPrice });
        }

        [HttpGet]
        [Route("removeItem")]
        public IActionResult removeItem(int productID)
        {
            
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();

            var existingProduct = cart.Find(c => c.productID == productID);

            if (existingProduct == null)
            {
                return Json(new { success = false, message = "Failed to remove item from cart" });
            }

            cart.Remove(existingProduct);
            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

            return Json(new { success = true });

        }

        [HttpGet]
        [Route("UpdateQuantity")]
        public IActionResult UpdateQuantity(int productID, int newQuantity)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            var existingProduct = cart.FirstOrDefault(c => c.productID == productID);

            if (existingProduct == null)
            {
                return Json(new { success = false, message = "Product not found in cart" });
            }

            existingProduct.itemQuantity = newQuantity;

            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);

            var newTotalPrice = existingProduct.productPrice * existingProduct.itemQuantity;

            return Json(new { success = true, newTotalPrice });
        }

        [HttpGet]
        [Route("Total")]
        public IActionResult Total()
        {
            return GetTotalPrice();
        }

        [HttpPost]
        [Route("CreateOrder")]
        public IActionResult CreateOrder()
        {
            //// Get cart items from session
            //var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            //int id = 0;

            //foreach (OrderItem item in cart)
            //{
            //    id = item.orderID;

            //};


            //if (cart.Count == 0)
            //{
            //    return Json(new { success = false, message = "Your cart is empty." });
            //}



            //var orderDate = DateTime.Now.ToString("yyyy-MM-dd");
            //var status = "Pending";
            //var orderItems = cart;
            //var totalPrice = cart.Sum(item => item.itemQuantity * item.productPrice);



            //await _context.SaveChangesAsync();

            //// Clear the cart after placing the order
            //HttpContext.Session.SetObjectAsJson(orderSession, new List<OrderItem>());

            //return Json(new { success = true, orderId = orders.Id });


            // Retrieve the order from session
            //var order = HttpContext.Session.GetObjectFromJson<Order>(orderSession);
            //if (order == null)
            //{
            //    return Json(new { success = false, message = "No active order." });
            //}

            // Retrieve the cart items from the session
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            var order = HttpContext.Session.GetObjectFromJson<Order>(orderSession);



            if (cart.Count == 0)
            {
                return Json(new { success = false, message = "Your cart is empty." });
            }

            if (order.orderItems == null)
            {
                order.orderItems = new List<OrderItem>();
            }

            // Update the order details
            foreach (OrderItem item in cart)
            {
                //order.orderItems.Add(item);
                order.orderItems.Add(new OrderItem
                {
                    orderID = order.Id,
                    productID = item.productID,
                    itemQuantity = item.itemQuantity,
                    productPrice = item.productPrice
                });
            }
            //order.orderDate = DateTime.Now.ToString("yyyy-MM-dd");
            order.totalPrice = cart.Sum(item => item.itemQuantity * item.productPrice);
            //order.status = "Pending";


            //// Save changes to the database


            HttpContext.Session.SetObjectAsJson(orderSession, order);



            // Clear the cart and order session after placing the order
            //HttpContext.Session.SetObjectAsJson(CartSessionKey, new List<OrderItem>());
            //HttpContext.Session.Remove(orderSession);

            // Return the order ID for further processing, such as redirecting to the OrderDetails page
            return Json(new { success = true, orderId = order.Id });
        }

        [HttpGet]
        [Route("OrderDetails")]
        public IActionResult OrderDetails()
        {
            //var order = await _context.orders
            //    .Include(o => o.orderItems)
            //    .FirstOrDefaultAsync(o => o.Id == orderId);

            //if (order == null)
            //{
            //    return NotFound();
            //}

            //return View(order);

            var order = HttpContext.Session.GetObjectFromJson<Order>(orderSession);
            //_context.orders.Add(order);
            //_context.SaveChanges();

            if (order == null)
            {
                return NotFound("Order not found in session.");
            }

            return View(order);
        }


        [HttpPost]
        [Route("PlaceOrder")]
        public IActionResult PlaceOrder()
        {
            //return Json(new { success = false, message = "Order not found." });
            // Assuming you have a method to get the current order

            

            var order = HttpContext.Session.GetObjectFromJson<Order>("Orders");

            if (order == null)
            {
                return Json(new { success = false, message = "Order not found." });
            }

            // Generate QR code (using a library like QRCoder)
            // You need to implement this method

            // Send email with the QR code
            var emailSent = SendOrderConfirmationEmail(); // Implement this method

            if (emailSent == null)
            {
                return Json(new { success = false, message = "Failed to send email." });
            }

            // Clear the session after placing the order
            //HttpContext.Session.Remove("CartItems");

            return Json(new { success = true });
        }

        // Example of how to generate a QR code (using QRCoder library)
        private string GenerateQrCode()
        {
            string qrCodeUrl = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=Order";
            return qrCodeUrl;
        }

        // Example of how to send an email (using your email service)
        private bool SendOrderConfirmationEmail()
        {
            string qrCodeUrl = GenerateQrCode();
            string emailBody = $"<p>Thank you for your order! Here is your QR code:</p><img src='{qrCodeUrl}' />";
            var fromAddress = new MailAddress("thetechonomicpost@gmail.com", "foodApp");
            const string fromPassword = "jtde diti xxal cmwn";
            var toEmail = HttpContext.Session.GetString("EMAIL");
            var toAddress = new MailAddress(toEmail);

            try
            {
                var client = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    EnableSsl = true
                };

                client.SendMailAsync(new MailMessage(fromAddress, toAddress)
                {
                    Subject = "Your Order Confirmation",
                    Body = emailBody,
                    IsBodyHtml = true
                });



                return true;
            }
            catch (Exception ex)
            {
                // Handle error (log it, show a message, etc.)
                return false;
            }
        }

    }
}
