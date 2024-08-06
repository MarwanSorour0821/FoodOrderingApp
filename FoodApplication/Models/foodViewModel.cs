using System;
namespace FoodApplication.Models
{
	public class foodViewModel
	{

        public string productName { get; set; }
        public string productImage { get; set; }
        public int productPrice { get; set; }
        public string productInformation { get; set; }
        public string productCategory { get; set; }

        public List<Product> FoodItems { get; set; } 

        public foodViewModel()
		{
		}
	}
}

