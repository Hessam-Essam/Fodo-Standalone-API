using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fodo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixBranchesToBeIntAgainq : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserBranches_UserId_BranchId",
                table: "UserBranches");

            migrationBuilder.CreateIndex(
                name: "IX_UserBranches_UserId",
                table: "UserBranches",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserBranches_UserId",
                table: "UserBranches");

            migrationBuilder.CreateIndex(
                name: "IX_UserBranches_UserId_BranchId",
                table: "UserBranches",
                columns: new[] { "UserId", "BranchId" },
                unique: true);
        }
    }
}
