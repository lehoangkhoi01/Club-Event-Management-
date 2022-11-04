using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClubProfiles",
                columns: table => new
                {
                    ClubProfileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClubName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalMember = table.Column<int>(type: "int", nullable: false),
                    FoundationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SocialAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubProfiles", x => x.ClubProfileId);
                });

            migrationBuilder.CreateTable(
                name: "EventCategories",
                columns: table => new
                {
                    EventCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategories", x => x.EventCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "EventStatuses",
                columns: table => new
                {
                    EventStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventStatusName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStatuses", x => x.EventStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventStartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventEndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsInternal = table.Column<bool>(type: "bit", nullable: false),
                    EventStatusId = table.Column<int>(type: "int", nullable: true),
                    EventCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_EventCategories_EventCategoryId",
                        column: x => x.EventCategoryId,
                        principalTable: "EventCategories",
                        principalColumn: "EventCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_EventStatuses_EventStatusId",
                        column: x => x.EventStatusId,
                        principalTable: "EventStatuses",
                        principalColumn: "EventStatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventActivities",
                columns: table => new
                {
                    EventActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventActivityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventActivities", x => x.EventActivityId);
                    table.ForeignKey(
                        name: "FK_EventActivities_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventClubProfile",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false),
                    ClubProfileId = table.Column<int>(type: "int", nullable: false),
                    IsOwner = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventClubProfile", x => new { x.EventId, x.ClubProfileId });
                    table.ForeignKey(
                        name: "FK_EventClubProfile_ClubProfiles_ClubProfileId",
                        column: x => x.ClubProfileId,
                        principalTable: "ClubProfiles",
                        principalColumn: "ClubProfileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventClubProfile_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventPosts",
                columns: table => new
                {
                    EventPostId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPosts", x => x.EventPostId);
                    table.ForeignKey(
                        name: "FK_EventPosts_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminAccounts",
                columns: table => new
                {
                    AdminAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIdentityEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminAccounts", x => x.AdminAccountId);
                    table.ForeignKey(
                        name: "FK_AdminAccounts_Users_UserIdentityEmail",
                        column: x => x.UserIdentityEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StudentAccounts",
                columns: table => new
                {
                    StudentAccountId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserIdentityEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAccounts", x => x.StudentAccountId);
                    table.ForeignKey(
                        name: "FK_StudentAccounts_Users_UserIdentityEmail",
                        column: x => x.UserIdentityEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClubProfileStudentAccount",
                columns: table => new
                {
                    ClubProfileId = table.Column<int>(type: "int", nullable: false),
                    StudentAccountId = table.Column<int>(type: "int", nullable: false),
                    CanModify = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClubProfileStudentAccount", x => new { x.StudentAccountId, x.ClubProfileId });
                    table.ForeignKey(
                        name: "FK_ClubProfileStudentAccount_ClubProfiles_ClubProfileId",
                        column: x => x.ClubProfileId,
                        principalTable: "ClubProfiles",
                        principalColumn: "ClubProfileId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClubProfileStudentAccount_StudentAccounts_StudentAccountId",
                        column: x => x.StudentAccountId,
                        principalTable: "StudentAccounts",
                        principalColumn: "StudentAccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminAccounts_UserIdentityEmail",
                table: "AdminAccounts",
                column: "UserIdentityEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ClubProfileStudentAccount_ClubProfileId",
                table: "ClubProfileStudentAccount",
                column: "ClubProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_EventActivities_EventId",
                table: "EventActivities",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventClubProfile_ClubProfileId",
                table: "EventClubProfile",
                column: "ClubProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_EventPosts_EventId",
                table: "EventPosts",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventCategoryId",
                table: "Events",
                column: "EventCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventStatusId",
                table: "Events",
                column: "EventStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAccounts_UserIdentityEmail",
                table: "StudentAccounts",
                column: "UserIdentityEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminAccounts");

            migrationBuilder.DropTable(
                name: "ClubProfileStudentAccount");

            migrationBuilder.DropTable(
                name: "EventActivities");

            migrationBuilder.DropTable(
                name: "EventClubProfile");

            migrationBuilder.DropTable(
                name: "EventPosts");

            migrationBuilder.DropTable(
                name: "StudentAccounts");

            migrationBuilder.DropTable(
                name: "ClubProfiles");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "EventCategories");

            migrationBuilder.DropTable(
                name: "EventStatuses");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
