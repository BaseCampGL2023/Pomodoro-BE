using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pomodoro.DataAccess.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FrequencyTypes",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { 1, "None" },
                    { 2, "Day" },
                    { 3, "Week" },
                    { 4, "Month" },
                    { 5, "Year" },
                    { 6, "Workday" },
                    { 7, "Weekend" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[,]
                {
                    { 1, "bob@gmail.com", "Bob" },
                    { 2, "jane@gmail.com", "Jane" }
                });

            migrationBuilder.InsertData(
                table: "Frequencies",
                columns: new[] { "Id", "Every", "FrequencyTypeId", "IsCustom" },
                values: new object[,]
                {
                    { 1, (short)0, 1, false },
                    { 2, (short)1, 2, false },
                    { 3, (short)1, 3, false },
                    { 4, (short)1, 4, false },
                    { 5, (short)1, 5, false },
                    { 6, (short)1, 6, false },
                    { 7, (short)1, 7, false }
                });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Id", "AutostartEnabled", "LongBreak", "PomodoroDuration", "PomodorosBeforeLongBreak", "ShortBreak", "UserId" },
                values: new object[,]
                {
                    { 1, true, (byte)10, (byte)30, (byte)2, (byte)5, 1 },
                    { 2, false, (byte)20, (byte)60, (byte)3, (byte)10, 2 }
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "AllocatedTime", "FrequencyId", "InitialDate", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, (short)100, 1, new DateTime(2022, 12, 27, 14, 59, 44, 291, DateTimeKind.Utc).AddTicks(4925), "Investigate Docker", 1 },
                    { 2, (short)200, 3, new DateTime(2022, 12, 27, 14, 59, 44, 291, DateTimeKind.Utc).AddTicks(4929), "Cleaning", 1 },
                    { 3, (short)120, 1, new DateTime(2022, 12, 27, 14, 59, 44, 291, DateTimeKind.Utc).AddTicks(4931), "Generate DB", 2 },
                    { 4, (short)60, 2, new DateTime(2022, 12, 27, 14, 59, 44, 291, DateTimeKind.Utc).AddTicks(4932), "Workout", 2 }
                });

            migrationBuilder.InsertData(
                table: "CompletedTasks",
                columns: new[] { "Id", "ActualDate", "PomodorosCount", "TaskId", "TimeSpent" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 12, 27, 14, 59, 44, 291, DateTimeKind.Utc).AddTicks(4945), 4f, 1, 120f },
                    { 2, new DateTime(2022, 12, 27, 14, 59, 44, 291, DateTimeKind.Utc).AddTicks(4949), 5.3f, 2, 160f },
                    { 3, new DateTime(2022, 12, 27, 14, 59, 44, 291, DateTimeKind.Utc).AddTicks(4950), 1.7f, 3, 100f },
                    { 4, new DateTime(2022, 12, 27, 14, 59, 44, 291, DateTimeKind.Utc).AddTicks(4951), 1f, 4, 60f }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompletedTasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CompletedTasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CompletedTasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CompletedTasks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Frequencies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Frequencies",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Frequencies",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Frequencies",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Settings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FrequencyTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FrequencyTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FrequencyTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FrequencyTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Frequencies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Frequencies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Frequencies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FrequencyTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FrequencyTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FrequencyTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
