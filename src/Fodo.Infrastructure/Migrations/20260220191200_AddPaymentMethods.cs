using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fodo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentMethods : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modifiers_OrderItems_OrderItemsOrderItemId",
                table: "Modifiers");

            migrationBuilder.DropIndex(
                name: "IX_Modifiers_OrderItemsOrderItemId",
                table: "Modifiers");

            migrationBuilder.DropColumn(
                name: "OrderItemsOrderItemId",
                table: "Modifiers");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Payment_methods",
                schema: "dbo",
                columns: table => new
                {
                    payment_method_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    client_id = table.Column<int>(type: "int", nullable: false),
                    method_name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    method_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment_methods", x => x.payment_method_id);
                    table.ForeignKey(
                        name: "FK_Payment_methods_Clients_client_id",
                        column: x => x.client_id,
                        principalTable: "Clients",
                        principalColumn: "client_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "dbo",
                columns: table => new
                {
                    PaymentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    Payment_method_id = table.Column<int>(type: "int", nullable: true),
                    Method = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SyncedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSynced = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Payment_methods_Payment_method_id",
                        column: x => x.Payment_method_id,
                        principalSchema: "dbo",
                        principalTable: "Payment_methods",
                        principalColumn: "payment_method_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payment_methods_client_id_is_active",
                schema: "dbo",
                table: "Payment_methods",
                columns: new[] { "client_id", "is_active" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DeviceId_CreatedAt",
                schema: "dbo",
                table: "Payments",
                columns: new[] { "DeviceId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                schema: "dbo",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Payment_method_id",
                schema: "dbo",
                table: "Payments",
                column: "Payment_method_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Payment_methods",
                schema: "dbo");

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
    }
}
