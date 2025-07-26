using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServerAspNetCoreAPIMakePC.Migrations
{
    /// <inheritdoc />
    public partial class AddDecimalPrecisionToPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,4)",
                nullable: false,
                comment: "Price of the product.",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComment: "Price of the product.");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "decimal(18,4)",
                nullable: false,
                comment: "The total price of the order.",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComment: "The total price of the order.");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "decimal(18,4)",
                nullable: false,
                comment: "The price per unit of the product in the order item.",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldComment: "The price per unit of the product in the order item.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                comment: "Price of the product.",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldComment: "Price of the product.");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                comment: "The total price of the order.",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldComment: "The total price of the order.");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitPrice",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                comment: "The price per unit of the product in the order item.",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)",
                oldComment: "The price per unit of the product in the order item.");
        }
    }
}
