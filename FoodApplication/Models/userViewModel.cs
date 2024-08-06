using System;
using System.ComponentModel.DataAnnotations;

namespace FoodApplication.Models
{
	public class userViewModel
	{
		//[Required]
		//public int id;

		[Required]
        public int EmployeeID { get; set; }

		[Required]
        public string password { get; set; }
		//public string newPassword { get; set; }

        public userViewModel()
		{
		}
	}
}

