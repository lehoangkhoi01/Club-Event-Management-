using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class renameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminAccounts_Users_UserIdentityEmail",
                table: "AdminAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAccounts_Users_UserIdentityEmail",
                table: "StudentAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "UserIdentities");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleId",
                table: "UserIdentities",
                newName: "IX_UserIdentities_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserIdentities",
                table: "UserIdentities",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminAccounts_UserIdentities_UserIdentityEmail",
                table: "AdminAccounts",
                column: "UserIdentityEmail",
                principalTable: "UserIdentities",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAccounts_UserIdentities_UserIdentityEmail",
                table: "StudentAccounts",
                column: "UserIdentityEmail",
                principalTable: "UserIdentities",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserIdentities_Roles_RoleId",
                table: "UserIdentities",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminAccounts_UserIdentities_UserIdentityEmail",
                table: "AdminAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAccounts_UserIdentities_UserIdentityEmail",
                table: "StudentAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserIdentities_Roles_RoleId",
                table: "UserIdentities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserIdentities",
                table: "UserIdentities");

            migrationBuilder.RenameTable(
                name: "UserIdentities",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_UserIdentities_RoleId",
                table: "Users",
                newName: "IX_Users_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Email");

            migrationBuilder.AddForeignKey(
                name: "FK_AdminAccounts_Users_UserIdentityEmail",
                table: "AdminAccounts",
                column: "UserIdentityEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAccounts_Users_UserIdentityEmail",
                table: "StudentAccounts",
                column: "UserIdentityEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "RoleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
