using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BurgerManiaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ordermodelfinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Burgers_BurgerId",
                schema: "OrderItem",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_BurgerId",
                schema: "OrderItem",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "Price",
                schema: "OrderItem",
                table: "OrderItems");

            migrationBuilder.AddColumn<string>(
                name: "BurgerName",
                schema: "OrderItem",
                table: "OrderItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BurgerName",
                schema: "OrderItem",
                table: "OrderItems");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                schema: "OrderItem",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_BurgerId",
                schema: "OrderItem",
                table: "OrderItems",
                column: "BurgerId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Burgers_BurgerId",
                schema: "OrderItem",
                table: "OrderItems",
                column: "BurgerId",
                principalSchema: "Burger",
                principalTable: "Burgers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
