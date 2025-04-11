using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExchangeDeadline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MarkedForScoring",
                table: "Authors");

            migrationBuilder.AddColumn<DateOnly>(
                name: "ExchangeDeadline",
                table: "Works",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeDeadline",
                table: "Works");

            migrationBuilder.AddColumn<bool>(
                name: "MarkedForScoring",
                table: "Authors",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
