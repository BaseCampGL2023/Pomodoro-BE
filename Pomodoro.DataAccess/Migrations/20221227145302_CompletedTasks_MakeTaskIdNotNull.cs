using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pomodoro.DataAccess.Migrations
{
    public partial class CompletedTasks_MakeTaskIdNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedTasks_Tasks_TaskId",
                table: "CompletedTasks");

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "CompletedTasks",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedTasks_Tasks_TaskId",
                table: "CompletedTasks",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletedTasks_Tasks_TaskId",
                table: "CompletedTasks");

            migrationBuilder.AlterColumn<int>(
                name: "TaskId",
                table: "CompletedTasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletedTasks_Tasks_TaskId",
                table: "CompletedTasks",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id");
        }
    }
}
