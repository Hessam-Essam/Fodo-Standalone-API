using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fodo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForignKeyClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Clients_ClientsClientId",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_ClientsClientId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "ClientsClientId",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "Client_id",
                table: "Branches",
                newName: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_client_id",
                table: "Branches",
                column: "client_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Clients_client_id",
                table: "Branches",
                column: "client_id",
                principalTable: "Clients",
                principalColumn: "client_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Clients_client_id",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Branches_client_id",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "Branches",
                newName: "Client_id");

            migrationBuilder.AddColumn<int>(
                name: "ClientsClientId",
                table: "Branches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_ClientsClientId",
                table: "Branches",
                column: "ClientsClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Clients_ClientsClientId",
                table: "Branches",
                column: "ClientsClientId",
                principalTable: "Clients",
                principalColumn: "client_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
