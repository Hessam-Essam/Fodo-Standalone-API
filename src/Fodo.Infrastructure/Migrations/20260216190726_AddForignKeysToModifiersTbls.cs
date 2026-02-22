using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fodo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForignKeysToModifiersTbls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Modifiers",
                newName: "client_id");

            migrationBuilder.RenameIndex(
                name: "IX_Modifiers_ClientId_ModifierGroupId",
                table: "Modifiers",
                newName: "IX_Modifiers_client_id_ModifierGroupId");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ModifierGroups",
                newName: "client_id");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ItemsModifierGroups",
                newName: "client_id");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsModifierGroups_ClientId_ItemId",
                table: "ItemsModifierGroups",
                newName: "IX_ItemsModifierGroups_client_id_ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ModifierGroups_client_id",
                table: "ModifierGroups",
                column: "client_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemsModifierGroups_Clients_client_id",
                table: "ItemsModifierGroups",
                column: "client_id",
                principalTable: "Clients",
                principalColumn: "client_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModifierGroups_Clients_client_id",
                table: "ModifierGroups",
                column: "client_id",
                principalTable: "Clients",
                principalColumn: "client_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Modifiers_Clients_client_id",
                table: "Modifiers",
                column: "client_id",
                principalTable: "Clients",
                principalColumn: "client_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemsModifierGroups_Clients_client_id",
                table: "ItemsModifierGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_ModifierGroups_Clients_client_id",
                table: "ModifierGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_Modifiers_Clients_client_id",
                table: "Modifiers");

            migrationBuilder.DropIndex(
                name: "IX_ModifierGroups_client_id",
                table: "ModifierGroups");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "Modifiers",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Modifiers_client_id_ModifierGroupId",
                table: "Modifiers",
                newName: "IX_Modifiers_ClientId_ModifierGroupId");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "ModifierGroups",
                newName: "ClientId");

            migrationBuilder.RenameColumn(
                name: "client_id",
                table: "ItemsModifierGroups",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemsModifierGroups_client_id_ItemId",
                table: "ItemsModifierGroups",
                newName: "IX_ItemsModifierGroups_ClientId_ItemId");
        }
    }
}
