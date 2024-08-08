using System;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;



namespace FoodApplication.Models
{
	public class User
	{
        
        public int Id { get; set; }

        
        public int EmployeeID { get; set; }
        public Employee employee { get; set; }
        public string password { get; set; }
        //public Order orderHistory { get; set; }
        public bool isFirstLogin { get; set; } = true;


        public int RoleID { get; set; }
        public Role role { get; set; }


        public ICollection<Order> orders;


        public User()
		{
       
		}
	}
}

