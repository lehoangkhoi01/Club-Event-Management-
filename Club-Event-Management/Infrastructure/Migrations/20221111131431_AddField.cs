﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class AddField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClubProfileId",
                table: "EventPosts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventPosts_ClubProfileId",
                table: "EventPosts",
                column: "ClubProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventPosts_ClubProfiles_ClubProfileId",
                table: "EventPosts",
                column: "ClubProfileId",
                principalTable: "ClubProfiles",
                principalColumn: "ClubProfileId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventPosts_ClubProfiles_ClubProfileId",
                table: "EventPosts");

            migrationBuilder.DropIndex(
                name: "IX_EventPosts_ClubProfileId",
                table: "EventPosts");

            migrationBuilder.DropColumn(
                name: "ClubProfileId",
                table: "EventPosts");
        }
    }
}
