using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pomodoro.DataAccess.Migrations
{
    public partial class InitWithFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FrequencyTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "varchar(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FrequencyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Frequencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FrequencyTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Every = table.Column<short>(type: "smallint", nullable: false),
                    IsCustom = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frequencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Frequencies_FrequencyTypes_FrequencyTypeId",
                        column: x => x.FrequencyTypeId,
                        principalTable: "FrequencyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PomodoroDuration = table.Column<byte>(type: "tinyint", nullable: false),
                    ShortBreak = table.Column<byte>(type: "tinyint", nullable: false),
                    LongBreak = table.Column<byte>(type: "tinyint", nullable: false),
                    PomodorosBeforeLongBreak = table.Column<byte>(type: "tinyint", nullable: false),
                    AutostartEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Settings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FrequencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    InitialDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    AllocatedTime = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Frequencies_FrequencyId",
                        column: x => x.FrequencyId,
                        principalTable: "Frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompletedTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActualDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    TimeSpent = table.Column<int>(type: "int", nullable: false),
                    PomodorosCount = table.Column<float>(type: "real", nullable: false),
                    IsDone = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedTasks_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FrequencyTypes",
                columns: new[] { "Id", "Value" },
                values: new object[,]
                {
                    { new Guid("00000001-0002-0003-0001-020304050607"), "None" },
                    { new Guid("00000002-0002-0003-0001-020304050607"), "Day" },
                    { new Guid("00000003-0002-0003-0001-020304050607"), "Week" },
                    { new Guid("00000004-0002-0003-0001-020304050607"), "Month" },
                    { new Guid("00000005-0002-0003-0001-020304050607"), "Year" },
                    { new Guid("00000006-0002-0003-0001-020304050607"), "Workday" },
                    { new Guid("00000007-0002-0003-0001-020304050607"), "Weekend" }
                });

            migrationBuilder.InsertData(
                table: "Frequencies",
                columns: new[] { "Id", "Every", "FrequencyTypeId", "IsCustom" },
                values: new object[,]
                {
                    { new Guid("00000002-0001-0003-0001-020304050607"), (short)0, new Guid("00000001-0002-0003-0001-020304050607"), false },
                    { new Guid("00000002-0002-0003-0001-020304050607"), (short)1, new Guid("00000002-0002-0003-0001-020304050607"), false },
                    { new Guid("00000002-0003-0003-0001-020304050607"), (short)1, new Guid("00000003-0002-0003-0001-020304050607"), false },
                    { new Guid("00000002-0004-0003-0001-020304050607"), (short)1, new Guid("00000004-0002-0003-0001-020304050607"), false },
                    { new Guid("00000002-0005-0003-0001-020304050607"), (short)1, new Guid("00000005-0002-0003-0001-020304050607"), false },
                    { new Guid("00000002-0006-0003-0001-020304050607"), (short)1, new Guid("00000006-0002-0003-0001-020304050607"), false },
                    { new Guid("00000002-0007-0003-0001-020304050607"), (short)1, new Guid("00000007-0002-0003-0001-020304050607"), false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedTasks_TaskId",
                table: "CompletedTasks",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Frequencies_FrequencyTypeId",
                table: "Frequencies",
                column: "FrequencyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FrequencyTypes_Value",
                table: "FrequencyTypes",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Settings_UserId",
                table: "Settings",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_FrequencyId",
                table: "Tasks",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedTasks");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Frequencies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "FrequencyTypes");
        }
    }
}
