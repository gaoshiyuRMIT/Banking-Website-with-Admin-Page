using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Banking.Migrations
{
    public partial class LockLogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Attempts",
                table: "Login",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockDateTime",
                table: "Login",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "Login");

            migrationBuilder.DropColumn(
                name: "LockDateTime",
                table: "Login");
        }
    }
}
