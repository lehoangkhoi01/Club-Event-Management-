using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClubProfiles_StudentAccounts_StudentAccountId",
                table: "ClubProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_ClubProfiles_ClubProfileId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ClubProfileId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_ClubProfiles_StudentAccountId",
                table: "ClubProfiles");

            migrationBuilder.DropColumn(
                name: "ClubProfileId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "StudentAccountId",
                table: "ClubProfiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubProfileId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StudentAccountId",
                table: "ClubProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ClubProfileId",
                table: "Events",
                column: "ClubProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ClubProfiles_StudentAccountId",
                table: "ClubProfiles",
                column: "StudentAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClubProfiles_StudentAccounts_StudentAccountId",
                table: "ClubProfiles",
                column: "StudentAccountId",
                principalTable: "StudentAccounts",
                principalColumn: "StudentAccountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_ClubProfiles_ClubProfileId",
                table: "Events",
                column: "ClubProfileId",
                principalTable: "ClubProfiles",
                principalColumn: "ClubProfileId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
