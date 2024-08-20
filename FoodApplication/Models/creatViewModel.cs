using System;
namespace FoodApplication.Models
{
	public class creatViewModel
	{
        public int id { get; set; }
        public int productStock { get; set; }
        public string productName { get; set; }
        public IFormFile Image { get; set; }
        public int productPrice { get; set; }
        public string productInformation { get; set; }
        public string productCategory { get; set; }
        

        

        public creatViewModel()
		{
		}
	}
}

