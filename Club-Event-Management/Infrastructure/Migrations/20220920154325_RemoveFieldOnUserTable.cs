using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class RemoveFieldOnUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminAccounts_Users_UserIdentityEmail",
                table: "AdminAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAccounts_Users_UserIdentityEmail",
                table: "StudentAccounts");

            migrationBuilder.DropColumn(
                name: "UserIdentityId",
                table: "StudentAccounts");

            migrationBuilder.DropColumn(
                name: "UserIdentityId",
                table: "AdminAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "UserIdentityEmail",
                table: "StudentAccounts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserIdentityEmail",
                table: "AdminAccounts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdminAccounts_Users_UserIdentityEmail",
                table: "AdminAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_StudentAccounts_Users_UserIdentityEmail",
                table: "StudentAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "UserIdentityEmail",
                table: "StudentAccounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserIdentityId",
                table: "StudentAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "UserIdentityEmail",
                table: "AdminAccounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserIdentityId",
                table: "AdminAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_AdminAccounts_Users_UserIdentityEmail",
                table: "AdminAccounts",
                column: "UserIdentityEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentAccounts_Users_UserIdentityEmail",
                table: "StudentAccounts",
                column: "UserIdentityEmail",
                principalTable: "Users",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
