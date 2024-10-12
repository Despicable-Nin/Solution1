using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp2.Migrations
{
    /// <inheritdoc />
    public partial class removedsomeentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SanitizedCrimeRecords");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUploaded",
                table: "Crimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 12, 19, 14, 11, 756, DateTimeKind.Local).AddTicks(2245),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 10, 19, 10, 45, 773, DateTimeKind.Local).AddTicks(7783));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateUploaded",
                table: "Crimes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 10, 10, 19, 10, 45, 773, DateTimeKind.Local).AddTicks(7783),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 10, 12, 19, 14, 11, 756, DateTimeKind.Local).AddTicks(2245));

            migrationBuilder.CreateTable(
                name: "SanitizedCrimeRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlcoholOrDrugInvolvement = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArrestDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArrestMade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CCTVCoverage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaseID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CrimeMotiveId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CrimeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Longitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedianIncome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NearbyLandmarkLatitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NearbyLandmarkLongitude = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PoliceDistrictId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PopulationDensityPerSqKm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProximityToPoliceStationInKm = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RawAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RawDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RawTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecurringIncident = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseTimeInMinutes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeverityId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreetLightPresent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnemploymentRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VictimCount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WeatherConditionId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SanitizedCrimeRecords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SanitizedCrimeRecords_CaseID",
                table: "SanitizedCrimeRecords",
                column: "CaseID",
                unique: true);
        }
    }
}
