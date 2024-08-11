using System;
namespace FoodApplication.Models
{
	public class forgotPasswordViewModel
	{
        public int EmployeeID { get; set; }
        public string Email { get; set; }
        public string OTP { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }


        public forgotPasswordViewModel()
		{
		}
	}
}

