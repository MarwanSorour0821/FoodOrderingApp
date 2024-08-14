using System;
namespace FoodApplication.Models
{
	public class Product
	{
		public int id { get; set; }
        public int productStock { get; set; }
        public string productName { get; set; }
        public string productImage { get; set; }
        public int productPrice { get; set; }
        public string productInformation { get; set; }
        public string productCategory { get; set; }
        //public int orderItemId { get; set; }

        public OrderItem orderItem { get; set; }

        
	}
}

