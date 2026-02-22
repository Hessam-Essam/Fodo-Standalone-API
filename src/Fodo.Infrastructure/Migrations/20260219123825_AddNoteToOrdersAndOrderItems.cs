using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fodo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNoteToOrdersAndOrderItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Orders",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "OrderItems",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrderItemsOrderItemId",
                table: "Modifiers",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modifiers_OrderItemsOrderItemId",
                table: "Modifiers",
                column: "OrderItemsOrderItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modifiers_OrderItems_OrderItemsOrderItemId",
                table: "Modifiers",
                column: "OrderItemsOrderItemId",
                principalTable: "OrderItems",
                principalColumn: "OrderItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modifiers_OrderItems_OrderItemsOrderItemId",
                table: "Modifiers");

            migrationBuilder.DropIndex(
                name: "IX_Modifiers_OrderItemsOrderItemId",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderItemsOrderItemId",
                table: "Modifiers");
        }
    }
}
