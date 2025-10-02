using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snap_N_Shop_API.Migrations
{
    /// <inheritdoc />
    public partial class refinedOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "OrderItems");
        }
    }
}
