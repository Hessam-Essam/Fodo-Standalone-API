using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fodo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCashInOutTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CashInVouchers",
                columns: table => new
                {
                    CashInVoucherId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SyncedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSynced = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashInVouchers", x => x.CashInVoucherId);
                });

            migrationBuilder.CreateTable(
                name: "CashOutVouchers",
                schema: "dbo",
                columns: table => new
                {
                    CashOutVoucherId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SyncedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSynced = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashOutVouchers", x => x.CashOutVoucherId);
                });

            migrationBuilder.CreateTable(
                name: "OrderRefunds",
                schema: "dbo",
                columns: table => new
                {
                    RefundId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RowGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false),
                    DeviceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SyncedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsSynced = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRefunds", x => x.RefundId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashOutVouchers_BranchId_ShiftId_CreatedAt",
                schema: "dbo",
                table: "CashOutVouchers",
                columns: new[] { "BranchId", "ShiftId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CashOutVouchers_DeviceId",
                schema: "dbo",
                table: "CashOutVouchers",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_CashOutVouchers_RowGuid",
                schema: "dbo",
                table: "CashOutVouchers",
                column: "RowGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderRefunds_BranchId_ShiftId_CreatedAt",
                schema: "dbo",
                table: "OrderRefunds",
                columns: new[] { "BranchId", "ShiftId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderRefunds_DeviceId",
                schema: "dbo",
                table: "OrderRefunds",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRefunds_OrderId",
                schema: "dbo",
                table: "OrderRefunds",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRefunds_RowGuid",
                schema: "dbo",
                table: "OrderRefunds",
                column: "RowGuid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashInVouchers");

            migrationBuilder.DropTable(
                name: "CashOutVouchers",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OrderRefunds",
                schema: "dbo");
        }
    }
}
