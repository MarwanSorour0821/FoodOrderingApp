using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FoodApplication.Migrations
{
    public partial class price_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "productPrice",
                table: "orderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "productPrice",
                table: "orderItems");
        }
    }
}
