using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinTrackUser4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_AspNetUsers_UserId",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Incomes_UserId",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "NextRecurrenceDate",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "RecurrenceInterval",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Incomes");

            migrationBuilder.CreateTable(
                name: "RecurringIncomes",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecurrenceInterval = table.Column<int>(type: "int", nullable: false),
                    NextRecurrenceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringIncomes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringIncomes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecurringIncomes_Incomes_Id",
                        column: x => x.Id,
                        principalTable: "Incomes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecurringIncomes_UserId",
                table: "RecurringIncomes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecurringIncomes");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Incomes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "NextRecurrenceDate",
                table: "Incomes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurrenceInterval",
                table: "Incomes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Incomes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_UserId",
                table: "Incomes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_AspNetUsers_UserId",
                table: "Incomes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
