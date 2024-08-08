﻿// <auto-generated />
using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FoodApplication.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240807142308_isFirstLoginAdded")]
    partial class isFirstLoginAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FoodApplication.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("companyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("employeeCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("phoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("employees");
                });

            modelBuilder.Entity("FoodApplication.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("orderDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("totalPrice")
                        .HasColumnType("int");

                    b.Property<int>("userID")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("userID");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("FoodApplication.Models.OrderItem", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"), 1L, 1);

                    b.Property<int>("itemQuantity")
                        .HasColumnType("int");

                    b.Property<int>("orderId")
                        .HasColumnType("int");

                    b.Property<int>("productID")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("orderId");

                    b.ToTable("orderItems");
                });

            modelBuilder.Entity("FoodApplication.Models.Product", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("int");

                    b.Property<string>("productCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("productImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("productInformation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("productName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("productPrice")
                        .HasColumnType("int");

                    b.Property<int>("productStock")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("products");
                });

            modelBuilder.Entity("FoodApplication.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("EmployeeID")
                        .HasColumnType("int");

                    b.Property<bool>("isFirstLogin")
                        .HasColumnType("bit");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("users");
                });

            modelBuilder.Entity("FoodApplication.Models.Employee", b =>
                {
                    b.HasOne("FoodApplication.Models.User", "user")
                        .WithOne("employee")
                        .HasForeignKey("FoodApplication.Models.Employee", "Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("FoodApplication.Models.Order", b =>
                {
                    b.HasOne("FoodApplication.Models.User", "user")
                        .WithMany("orders")
                        .HasForeignKey("userID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("FoodApplication.Models.OrderItem", b =>
                {
                    b.HasOne("FoodApplication.Models.Order", "order")
                        .WithMany("orderItems")
                        .HasForeignKey("orderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("order");
                });

            modelBuilder.Entity("FoodApplication.Models.Product", b =>
                {
                    b.HasOne("FoodApplication.Models.OrderItem", "orderItem")
                        .WithOne("product")
                        .HasForeignKey("FoodApplication.Models.Product", "id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("orderItem");
                });

            modelBuilder.Entity("FoodApplication.Models.Order", b =>
                {
                    b.Navigation("orderItems");
                });

            modelBuilder.Entity("FoodApplication.Models.OrderItem", b =>
                {
                    b.Navigation("product")
                        .IsRequired();
                });

            modelBuilder.Entity("FoodApplication.Models.User", b =>
                {
                    b.Navigation("employee")
                        .IsRequired();

                    b.Navigation("orders");
                });
#pragma warning restore 612, 618
        }
    }
}
