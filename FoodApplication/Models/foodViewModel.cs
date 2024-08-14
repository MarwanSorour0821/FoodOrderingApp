using System;
namespace FoodApplication.Models
{
	public class foodViewModel
	{

        public int id;
        public string productName { get; set; }
        public string productImage { get; set; }
        public int productPrice { get; set; }
        public string productInformation { get; set; }
        public string productCategory { get; set; }
        public string productQuantity { get; set; }

        public IEnumerable<Product> FoodItems { get; set; } 

        
	}
}

