using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pomodoro.DataAccess.Migrations
{
    public partial class SetRelationWithPomoIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PomoIdentityUserId",
                table: "AppUsers",
                type: "uniqueidentifier",
                nullable: false/*,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000")*/);

            migrationBuilder.CreateIndex(
                name: "IX_Users_AspNetUserId",
                table: "AppUsers",
                column: "PomoIdentityUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUsers_AspNetUsers_PomoIdentityUserId",
                table: "AppUsers",
                column: "PomoIdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppUsers_AspNetUsers_PomoIdentityUserId",
                table: "AppUsers");

            migrationBuilder.DropIndex(
                name: "IX_Users_AspNetUserId",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "PomoIdentityUserId",
                table: "AppUsers");
        }
    }
}
