using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Data.Migrations
{
    /// <inheritdoc />
    public partial class FinTrackUser3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Expenses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "NextRecurrenceDate",
                table: "Expenses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RecurrenceInterval",
                table: "Expenses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Expenses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_UserId",
                table: "Incomes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_UserId",
                table: "Expenses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_AspNetUsers_UserId",
                table: "Expenses",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_AspNetUsers_UserId",
                table: "Incomes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_AspNetUsers_UserId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_AspNetUsers_UserId",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Incomes_UserId",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_UserId",
                table: "Expenses");

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

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "NextRecurrenceDate",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "RecurrenceInterval",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Expenses");
        }
    }
}
