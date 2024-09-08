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
using Microsoft.AspNetCore.SignalR;

namespace FoodApplication.Controllers
{
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "CartItems";
        private const string orderSession = "Orders";
        private readonly IHubContext<NotificationHub> _hubContext;


        public CartController(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
            _context = context;
        }

        [HttpPost]
        [Route("AddProductToCartAsync")]
        public async Task<IActionResult> AddProductToCartAsync(int? id)
        {
            var product = await _context.products.FindAsync(id);
            var user = HttpContext.Session.GetInt32("ID");
            var userInfo = await _context.users.FirstOrDefaultAsync(o => o.Id == user);


            //Order newOrder = new Order();
            //newOrder.userID = (int)userInfo.Id;

            //try
            //{
            //    _context.orders.Add(newOrder);
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateException ex)
            //{
            //    var innerException = ex.InnerException?.Message;
            //    return Json(new { success = false, message = $"Could not add product to cart: {innerException}" });
            //}

        //    var newOrder = await _context.orders
        //.Where(o => o.userID == userInfo.Id && o.status == "Pending")
        //.OrderByDescending(o => o.orderDate)
        //.FirstOrDefaultAsync();

            // If no existing pending order, create a new one
            //if (newOrder == null)
            //{
            //    newOrder = new Order
            //    {
            //        userID = userInfo.Id,
            //        orderDate = DateTime.Now.ToString("yyyy-MM-dd"),
            //        status = "Pending",
            //        totalPrice = 0 // This will be updated when items are added
            //    };

            //    try
            //    {
            //        _context.orders.Add(newOrder);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateException ex)
            //    {
            //        var innerException = ex.InnerException?.Message;
            //        return Json(new { success = false, message = $"Could not add product to cart: {innerException}" });
            //    }// Save the new order
            //}


            if (product == null)
            {
                return Json(new { success = false, message = "Product Not Found" });
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            var order = HttpContext.Session.GetObjectFromJson<Order>(orderSession) ?? new Order
            {
                //Id = newOrder.Id,

                orderDate = DateTime.Now.ToString("yyyy-MM-dd"),
                status = "Pending",
                orderItems = new List<OrderItem>()
            };

            //var lastOrder = await _context.orders
            //                      .Where(o => o.userID == userInfo.Id)
            //                      .OrderByDescending(o => o.orderDate)
            //                      .FirstOrDefaultAsync();

            var existingProduct = cart.Find(c => c.productID == id);

            if (existingProduct != null)
            {
                existingProduct.itemQuantity++;
            }
            else if (product.productStock < 1)
            {
                return Json(new { success = false, message = "Quantity more than stock" });
            }
            else
            {
                cart.Add(new OrderItem
                {
                    //orderID = lastOrder.Id,
                    productID = product.id,
                    productName = product.productName,
                    itemQuantity = 1,
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
            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            return View(cart);
        }


        [HttpGet]
        [Route("GetCartCount")]
        public int GetCartCount()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            int itemCount = cart.Sum(item => item.itemQuantity); // If each OrderItem has a Quantity property

            // Return the total count of items in the cart
            return itemCount;
            
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
            // Retrieve the cart items from the session
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            var order = HttpContext.Session.GetObjectFromJson<Order>(orderSession);

            var user = HttpContext.Session.GetInt32("ID");
            var userInfo = _context.users.FirstOrDefault(o => o.Id == user);

            

            //var lastOrder = _context.orders
            //                      .Where(o => o.userID == userInfo.Id)
            //                      .OrderByDescending(o => o.orderDate)
            //                      .FirstOrDefault();

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
                    //orderID = lastOrder.Id,
                    productID = item.productID,
                    productName = item.productName,
                    itemQuantity = item.itemQuantity,
                    productPrice = item.productPrice
                });
            }
            //order.orderDate = DateTime.Now.ToString("yyyy-MM-dd");
            order.totalPrice = cart.Sum(item => item.itemQuantity * item.productPrice);
            //order.status = "Pending";

            //lastOrder.orderDate = DateTime.Now.ToString("yyyy-MM-dd");
            //lastOrder.status = "Pending";
            //lastOrder.totalPrice = cart.Sum(item => item.itemQuantity * item.productPrice);

            //_context.orders.Update(lastOrder);
            //_context.SaveChanges();

            HttpContext.Session.SetObjectAsJson(orderSession, order);

            // Return the order ID for further processing, such as redirecting to the OrderDetails page
            return Json(new { success = true, orderId = order.Id });
        }

        [HttpGet]
        [Route("OrderDetails")]
        public IActionResult OrderDetails()
        {

            if (HttpContext.Session.GetString("EMAIL") == null)
            {
                return RedirectToAction("Index", "Login");
            }

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
            var order = HttpContext.Session.GetObjectFromJson<Order>("Orders");
            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            var user = HttpContext.Session.GetInt32("ID");
            var userInfo = _context.users.FirstOrDefault(o => o.Id == user);

            Order newOrder = new Order();
            newOrder.userID = userInfo.EmployeeID;
            newOrder.orderDate = DateTime.Now.ToString("yyyy-MM-dd");
            newOrder.status = "Pending";
            newOrder.totalPrice = cart.Sum(item => item.itemQuantity * item.productPrice);
            newOrder.orderNumber = GenerateOrderNumber();


            _context.orders.Add(newOrder);
            _context.SaveChanges();

            //    {
            //        userID = userInfo.Id,
            //        orderDate = DateTime.Now.ToString("yyyy-MM-dd"),
            //        status = "Pending",
            //        totalPrice = 0 // This will be updated when items are added
            //    };

            var lastOrder = _context.orders
                                  .Where(o => o.userID == userInfo.Id)
                                  .OrderByDescending(o => o.Id)
                                  .FirstOrDefault();
            

            if (order == null)
            {
                return Json(new { success = false, message = "Order not found." });
            }

            // Send email with the QR code
            var emailSent = SendOrderConfirmationEmail(lastOrder); // Implement this method
            //var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            if (emailSent == null)
            {
                return Json(new { success = false, message = "Failed to send email." });
            }

            // Clear the session after placing the order
            //HttpContext.Session.Remove("CartItems");

            foreach(OrderItem item in cart)
            {
                var product = _context.products.SingleOrDefault(a => a.id == item.productID);
                product.productStock = product.productStock - item.itemQuantity;
                _context.Update(product);
                _context.SaveChanges();
            }

            _hubContext.Clients.All.SendAsync("ReceiveOrderUpdate", "A new order has been placed.");

            //return Json(new { success = true });
            return RedirectToAction("login1", "Login");
        }

        // Example of how to generate a QR code (using QRCoder library)
        

        // Example of how to send an email (using your email service)
        private bool SendOrderConfirmationEmail(Order order)
        {
            //string qrCodeUrl = GenerateQrCode(order);
            //string emailBody = $"<p>Thank you for your order! The estimated time for your order is from 45-60 minutes. Here is your QR code:</p><img src='{qrCodeUrl}' />";
            string emailBody = $"<p>Thank you for your order! The estimated time for your order is from 45-60 minutes. Your order number is {order.orderNumber}. You'll be confirming this number on delivery.";
            var fromAddress = new MailAddress("thetechonomicpost@gmail.com", "Butler & Co.");
            const string fromPassword = "jtde diti xxal cmwn";
            var toEmail = HttpContext.Session.GetString("EMAIL");
            var toAddress = new MailAddress(toEmail);

            
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

        //GENERATE ORDER NUMBER
        private int GenerateOrderNumber()
        {
            // Get the last order placed
            var lastOrder = _context.orders
                                          .OrderByDescending(o => o.Id)
                                          .FirstOrDefault();

            int newOrderNumber = 1; // Default to 1 if no orders exist or it's a new day

            if (lastOrder != null)
            {
                // Parse the last order's date (ensure date format matches)
                DateTime lastOrderDate = DateTime.Parse(lastOrder.orderDate);

                // Check if the last order was placed on the current date
                if (lastOrderDate.Date == DateTime.UtcNow.Date)
                {
                    // If it's the same day, increment the order number
                    newOrderNumber = lastOrder.orderNumber + 1;
                }
                // If it's a new day, newOrderNumber will remain 1 (reset)
            }

            return newOrderNumber;
        }


    }
}
