using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp2.Migrations
{
    /// <inheritdoc />
    public partial class addbatchidtocrimetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUploaded",
                table: "Crimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 10, 19, 10, 45, 773, DateTimeKind.Local).AddTicks(7783),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 10, 15, 19, 23, 821, DateTimeKind.Local).AddTicks(9528));

            migrationBuilder.AddColumn<Guid>(
                name: "BatchId",
                table: "Crimes",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "Crimes");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUploaded",
                table: "Crimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 10, 15, 19, 23, 821, DateTimeKind.Local).AddTicks(9528),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 10, 19, 10, 45, 773, DateTimeKind.Local).AddTicks(7783));
        }
    }
}
