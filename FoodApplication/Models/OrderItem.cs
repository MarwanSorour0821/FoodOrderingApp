using System;
using System.ComponentModel.DataAnnotations;

namespace FoodApplication.Models
{
	public class OrderItem
	{
        public int id { get; set; }

        public int productID { get; set; }
        public Product product { get; set; }

        public int itemQuantity { get; set; }

        public int productPrice { get; set; }
        
        public int orderID;
        public Order order { get; set; }

        
		
	}
}

