using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fodo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForignKeyToCashInOutTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CashOutVouchers_ShiftId",
                schema: "dbo",
                table: "CashOutVouchers",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_CashOutVouchers_UserId",
                schema: "dbo",
                table: "CashOutVouchers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CashInVouchers_BranchId",
                table: "CashInVouchers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CashInVouchers_ShiftId",
                table: "CashInVouchers",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_CashInVouchers_UserId",
                table: "CashInVouchers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashInVouchers_Branches_BranchId",
                table: "CashInVouchers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CashInVouchers_Shifts_ShiftId",
                table: "CashInVouchers",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CashInVouchers_Users_UserId",
                table: "CashInVouchers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashOutVouchers_Branches_BranchId",
                schema: "dbo",
                table: "CashOutVouchers",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "BranchId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CashOutVouchers_Shifts_ShiftId",
                schema: "dbo",
                table: "CashOutVouchers",
                column: "ShiftId",
                principalTable: "Shifts",
                principalColumn: "ShiftId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CashOutVouchers_Users_UserId",
                schema: "dbo",
                table: "CashOutVouchers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashInVouchers_Branches_BranchId",
                table: "CashInVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_CashInVouchers_Shifts_ShiftId",
                table: "CashInVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_CashInVouchers_Users_UserId",
                table: "CashInVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_CashOutVouchers_Branches_BranchId",
                schema: "dbo",
                table: "CashOutVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_CashOutVouchers_Shifts_ShiftId",
                schema: "dbo",
                table: "CashOutVouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_CashOutVouchers_Users_UserId",
                schema: "dbo",
                table: "CashOutVouchers");

            migrationBuilder.DropIndex(
                name: "IX_CashOutVouchers_ShiftId",
                schema: "dbo",
                table: "CashOutVouchers");

            migrationBuilder.DropIndex(
                name: "IX_CashOutVouchers_UserId",
                schema: "dbo",
                table: "CashOutVouchers");

            migrationBuilder.DropIndex(
                name: "IX_CashInVouchers_BranchId",
                table: "CashInVouchers");

            migrationBuilder.DropIndex(
                name: "IX_CashInVouchers_ShiftId",
                table: "CashInVouchers");

            migrationBuilder.DropIndex(
                name: "IX_CashInVouchers_UserId",
                table: "CashInVouchers");
        }
    }
}
