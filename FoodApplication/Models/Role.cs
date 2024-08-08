using System;
using System.ComponentModel.DataAnnotations;

namespace FoodApplication.Models
{
	public class Role
	{

		
		public int Id { get; set; }
		public string roleName { get; set; }
		public User user { get; set; }

		public Role()
		{
		}
	}
}

