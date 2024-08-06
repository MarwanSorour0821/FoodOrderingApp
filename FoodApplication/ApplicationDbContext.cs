using System;
using Microsoft.EntityFrameworkCore;
using FoodApplication.Models;



namespace Application
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> users { get; set; }
		public DbSet<Employee> employees { get; set; }
		public DbSet<Order> orders { get; set; }
		public DbSet<OrderItem> orderItems { get; set; }
		public DbSet<Product> products { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder modelbuilder)
		{

			//One-To-One Relationship with User and Employee
			modelbuilder.Entity<User>().HasOne(u => u.employee).WithOne(u => u.user).HasForeignKey<Employee>().OnDelete(DeleteBehavior.Restrict);

			//One-To-Many Relationship with User and Order
			modelbuilder.Entity<Order>().HasOne(u => u.user).WithMany(u => u.orders).OnDelete(DeleteBehavior.Restrict);

			//One-To-One Relationship with OrderItem and Product
			modelbuilder.Entity<OrderItem>().HasOne(p => p.product).WithOne(o => o.orderItem).HasForeignKey<Product>().OnDelete(DeleteBehavior.Restrict);

			//One-To-Many Relationship with Order and OrderItem
			modelbuilder.Entity<Order>().HasMany(o => o.orderItems).WithOne(o => o.order).OnDelete(DeleteBehavior.Restrict);


		}

		


		
	}
}

