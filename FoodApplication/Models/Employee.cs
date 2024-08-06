using System;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;


namespace FoodApplication.Models
{
	public class Employee
	{
		
		public int Id { get; set; }

		public User user { get; set; }

        public string Name { get; set; }

        public string phoneNumber { get; set; }

        public string Email { get; set; }

        public string employeeCode { get; set; }

        public string companyName { get; set; }

        //public ICollection<User> users { get; set; }

        public Employee()
		{
		}
	}
}

