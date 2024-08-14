//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Application;
//using FoodApplication.Models;
//using Microsoft.AspNetCore.Mvc;
//using System.Collections.Generic;
//using FoodApplication.Helper;
//using Microsoft.EntityFrameworkCore;

//// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace FoodApplication.Controllers
//{
//    public class CartController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        private const string CartSessionKey = "CartItems";


//        public CartController(ApplicationDbContext context)
//        {
//            _context = context;
//        }


//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> AddProductToCartAsync(foodViewModel model)
//        {

//            OrderItem orderItem = new()
//            {
//                itemQuantity = 0,
//            };

//            var product = await _context.products.Where(u => u.productName == model.productName).FirstOrDefaultAsync(); ;

//            if (product == null)
//            {
//                return Json(new { success = false, message = "Product Not Found" });
//            }

//            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();

//            var existingProduct = cart.Find(c => c.productID == model.id);

//            if (existingProduct != null)
//            {
//                existingProduct.itemQuantity++;
//            }
//            else
//            {
//                cart.Add(new OrderItem
//                {
//                    productID = product.id,
//                    itemQuantity = 1,

//                });
//            }

//            HttpContext.Session.SetObjectAsJson(CartSessionKey, cart);


//            return Json(new { success = true });
//        }

//        public IActionResult Index()
//        {
//            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
//            return View(cart);
//        }

//        //// GET: /<controller>/
//        //public IActionResult Index()
//        //{
//        //    return View();
//        //}
//    }
//}

using System.Linq;
using System.Threading.Tasks;
using FoodApplication.Models;
using Microsoft.AspNetCore.Mvc;
using FoodApplication.Helper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Application;

namespace FoodApplication.Controllers
{
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartSessionKey = "CartItems";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("AddProductToCartAsync")]
        public async Task<IActionResult> AddProductToCartAsync(int? id)
        {
            //var product = await _context.products.Where(u => u.productName == model.productName).FirstOrDefaultAsync();
            var product = await _context.products.FindAsync(id);

            if (product == null)
            {
                return Json(new { success = false, message = "Product Not Found" });
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<OrderItem>>(CartSessionKey) ?? new List<OrderItem>();
            var existingProduct = cart.Find(c => c.productID == id);

            if (existingProduct != null)
            {
                existingProduct.itemQuantity++;
            }
            else
            {
                cart.Add(new OrderItem
                {
                    productID = product.id,
                    itemQuantity = 1,
                    productPrice = product.productPrice
                });
            }

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
        [Route("Total")]
        public IActionResult Total()
        {
            return GetTotalPrice();
        }
    }
}
