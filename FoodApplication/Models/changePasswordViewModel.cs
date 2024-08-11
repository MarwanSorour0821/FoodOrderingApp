using System;
namespace FoodApplication.Models
{
	public class changePasswordViewModel
	{
		public int EmployeeID;
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }

        public changePasswordViewModel()
		{
		}
	}
}

