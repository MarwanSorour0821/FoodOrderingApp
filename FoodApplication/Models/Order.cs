using System;
namespace FoodApplication.Models
{
	public class Order
	{

		public int Id { get; set; }
        public string orderDate { get; set; }
        public int totalPrice { get; set; }
        public int userID { get; set; }
        public User user { get; set; }
        public string status { get; set; }

        public ICollection<OrderItem> orderItems { get; set; }

        public Order()
		{
		}
	}
}

