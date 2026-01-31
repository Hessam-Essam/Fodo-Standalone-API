using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fodo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixBranchesToBeInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBranches_Users_UsersUserId",
                table: "UserBranches");

            migrationBuilder.RenameColumn(
                name: "UsersUserId",
                table: "UserBranches",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_UserBranches_UsersUserId",
                table: "UserBranches",
                newName: "IX_UserBranches_UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBranches_Users_UserId1",
                table: "UserBranches",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserBranches_Users_UserId1",
                table: "UserBranches");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "UserBranches",
                newName: "UsersUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserBranches_UserId1",
                table: "UserBranches",
                newName: "IX_UserBranches_UsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserBranches_Users_UsersUserId",
                table: "UserBranches",
                column: "UsersUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
