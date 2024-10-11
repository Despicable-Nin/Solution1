using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp2.Migrations
{
    /// <inheritdoc />
    public partial class renamefatcluster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeatherCondition",
                table: "SanitizedCrimeRecords",
                newName: "WeatherConditionId");

            migrationBuilder.RenameColumn(
                name: "Severity",
                table: "SanitizedCrimeRecords",
                newName: "SeverityId");

            migrationBuilder.RenameColumn(
                name: "PoliceDistrict",
                table: "SanitizedCrimeRecords",
                newName: "RawTime");

            migrationBuilder.RenameColumn(
                name: "CrimeMotive",
                table: "SanitizedCrimeRecords",
                newName: "RawDate");

            migrationBuilder.AddColumn<string>(
                name: "CrimeMotiveId",
                table: "SanitizedCrimeRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PoliceDistrictId",
                table: "SanitizedCrimeRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RawAddress",
                table: "SanitizedCrimeRecords",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUploaded",
                table: "Crimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 10, 15, 19, 23, 821, DateTimeKind.Local).AddTicks(9528),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 9, 26, 17, 37, 24, 516, DateTimeKind.Local).AddTicks(2181));

            migrationBuilder.AddColumn<string>(
                name: "CrimeMotiveId",
                table: "Crimes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CrimeTypeId",
                table: "Crimes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Latitude",
                table: "Crimes",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Longitude",
                table: "Crimes",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PoliceDistrictId",
                table: "Crimes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeverityId",
                table: "Crimes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WeatherConditionId",
                table: "Crimes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobType = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Retries = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropColumn(
                name: "CrimeMotiveId",
                table: "SanitizedCrimeRecords");

            migrationBuilder.DropColumn(
                name: "PoliceDistrictId",
                table: "SanitizedCrimeRecords");

            migrationBuilder.DropColumn(
                name: "RawAddress",
                table: "SanitizedCrimeRecords");

            migrationBuilder.DropColumn(
                name: "CrimeMotiveId",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "CrimeTypeId",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "PoliceDistrictId",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "SeverityId",
                table: "Crimes");

            migrationBuilder.DropColumn(
                name: "WeatherConditionId",
                table: "Crimes");

            migrationBuilder.RenameColumn(
                name: "WeatherConditionId",
                table: "SanitizedCrimeRecords",
                newName: "WeatherCondition");

            migrationBuilder.RenameColumn(
                name: "SeverityId",
                table: "SanitizedCrimeRecords",
                newName: "Severity");

            migrationBuilder.RenameColumn(
                name: "RawTime",
                table: "SanitizedCrimeRecords",
                newName: "PoliceDistrict");

            migrationBuilder.RenameColumn(
                name: "RawDate",
                table: "SanitizedCrimeRecords",
                newName: "CrimeMotive");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUploaded",
                table: "Crimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 9, 26, 17, 37, 24, 516, DateTimeKind.Local).AddTicks(2181),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 10, 15, 19, 23, 821, DateTimeKind.Local).AddTicks(9528));
        }
    }
}
