using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp2.Migrations
{
    /// <inheritdoc />
    public partial class addctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUploaded",
                table: "Crimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 24, 17, 15, 26, 970, DateTimeKind.Local).AddTicks(7363),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 23, 17, 40, 1, 920, DateTimeKind.Local).AddTicks(5529));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUploaded",
                table: "Crimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 23, 17, 40, 1, 920, DateTimeKind.Local).AddTicks(5529),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 24, 17, 15, 26, 970, DateTimeKind.Local).AddTicks(7363));
        }
    }
}
